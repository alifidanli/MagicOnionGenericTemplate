﻿using Grpc.Net.Client;
using LitJWT;
using LitJWT.Algorithms;
using MagicOnion.Serialization.MemoryPack;
using MagicOnion.Server;
using MagicT.Server.Database;
using MagicT.Server.Jwt;
using MessagePipe;
using Microsoft.EntityFrameworkCore;
using MagicT.Server.Exceptions;
using MagicT.Server.HostedServices;
using MagicT.Server.Initializers;
using MagicT.Server.ZoneTree;
using MagicT.Server.ZoneTree.Zones;
using MagicT.Shared.Models.ServiceModels;
using MagicT.Shared.Models;
using MagicT.Shared.Extensions;
using MagicT.Server.Handlers;
using MessagePipe.Interprocess.Workers;
using Coravel;
using MagicOnion;
using MagicT.Server.Services.Base;
using MagicT.Server.Invocables;
using MagicT.Server.Managers;
using MagicT.Server.Services;
using Org.BouncyCastle.Crypto;
using MagicT.Shared.Helpers;
using MagicT.Shared.Managers;

#if (GRPC_SSL)
using MagicT.Server.Helpers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
#endif

var builder = WebApplication.CreateBuilder(args);

#if (GRPC_SSL)
//*** Important Note : Make sure server.crt and server.key copyToOutputDirectory property is set to Always copy
var crtPath = Path.Combine(Environment.CurrentDirectory, builder.Configuration.GetSection("Certificate:CrtPath").Value);
var keyPath = Path.Combine(Environment.CurrentDirectory, builder.Configuration.GetSection("Certificate:KeyPath").Value);

var certificate = CertificateHelper.GetCertificate(crtPath,keyPath);


//Verify certificate
var verf = certificate.Verify();

builder.WebHost.ConfigureKestrel((context, opt) =>
{
    opt.ListenLocalhost(7197, o =>
    {
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps(certificate);
    });
});
#endif

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMagicOnion(x =>
{
#if  DEBUG
    x.IsReturnExceptionStackTraceInErrorDetail = true;
#endif
    //Remove this line to use magic onion with message pack
    x.MessageSerializer = MemoryPackMagicOnionSerializerProvider.Instance;
});
builder.Services.RegisterPipes();

builder.Services.AddSingleton<TokenManager>();

builder.Services.AddSingleton<AuthenticationManager>();

builder.Services.AddSingleton<DbExceptionHandler>();

builder.Services.AddQueue();

builder.Services.AddTransient(typeof(AuditFailedInvocable<>));

builder.Services.AddTransient(typeof(AuditRecordsInvocable<>));

builder.Services.AddTransient(typeof(AuditQueryInvocable<>));


builder.Services.AddDbContextPool<MagicTContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(MagicTContext))!));

//builder.Services.AddDbContext<MagicTContextAudit>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(MagicTContext))!));

builder.Services.AddScoped(typeof(DatabaseService<,,>));


var zonedbPath = builder.Configuration.GetSection("ZoneDbPath").Value;

builder.Services.AddSingleton(x => new UsersZoneDb(zonedbPath + $"/{nameof(UsersZoneDb)}"));
builder.Services.AddSingleton(x=> new UsedTokensZoneDb(zonedbPath+$"/{nameof(UsedTokensZoneDb)}"));
//builder.Services.AddSingleton(x => new PermissionsZoneDb(zonedbPath + $"/{nameof(PermissionsZoneDb)}"));

builder.Services.AddSingleton<IAsyncRequestHandler<int,string>, MyAsyncRequestHandler>();


builder.Services.AddSingleton<ZoneDbManager>();

builder.Services.AddSingleton<Lazy<List<PERMISSIONS>>>();

builder.Services.AddSingleton<IKeyExchangeManager, KeyExchangeManager>();

builder.Services.AddSingleton<KeyExchangeData>();

builder.Services.AddSingleton(_ =>
{
    var key = HS256Algorithm.GenerateRandomRecommendedKey();

    var encoder = new JwtEncoder(new HS256Algorithm(key));
    var decoder = new JwtDecoder(encoder.SignAlgorithm);

    return new MagicTTokenService
    {
        Encoder = encoder,
        Decoder = decoder
    };
});

// builder.Services.AddSingleton(provider => new MemoryDatabaseManager(provider));
builder.Services.AddScoped<DataInitializer>();

var app = builder.Build();

using var scope = app.Services.CreateAsyncScope();

var KeyExchangeManager = app.Services.GetRequiredService<IKeyExchangeManager>();

KeyExchangeManager.Initialize();

 
using var pipeWorker = app.Services.GetRequiredService<TcpWorker>();

pipeWorker.StartReceiver();



//var subscriber = app.Services.GetService<IDistributedSubscriber<string, USERS>>();

//await subscriber.SubscribeAsync("foobar", x =>
//{
//    Console.WriteLine("subscribed");
//});

scope.ServiceProvider.GetRequiredService<DataInitializer>().Initialize();
 
app.UseRouting();

app.MapMagicOnionHttpGateway("_", app.Services.GetService<MagicOnionServiceDefinition>().MethodHandlers,
    GrpcChannel.ForAddress("http://localhost:5029")); // Use HTTP instead of HTTPS
app.MapMagicOnionSwagger("swagger", app.Services.GetService<MagicOnionServiceDefinition>().MethodHandlers, "/_/");

app.MapMagicOnionService();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();