﻿using Generator.Components.Extensions;
using MagicT.Client.Extensions;
using MagicT.Client.Initializers;
using MagicT.Client.Services;
using MudBlazor.Services;
using MagicT.Shared.Extensions;
using MagicT.Shared.Hubs;
using MagicT.Shared.Services;
using MessagePipe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddMudServices();
builder.Services.AutoRegisterFromMagicTShared();
builder.Services.AutoRegisterFromMagicTClient();
builder.Services.AutoRegisterFromMagicTWebTemplate();
builder.Services.AutoRegisterFromMagicTWebShared();
builder.Services.RegisterGeneratorComponents();
builder.Services.RegisterClientServices(builder.Configuration);
builder.Services.RegisterShared(builder.Configuration);


builder.Services.AddHttpContextAccessor();
builder.Services.AddMessagePipe();

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
  

await Task.Delay(5000);
var app = builder.Build();

await using var scope =   app.Services.CreateAsyncScope();
var keyExchangeService = scope.ServiceProvider.GetService<IKeyExchangeService>() as KeyExchangeService;
await keyExchangeService?.GlobalKeyExchangeAsync()!;

var testHub = scope.ServiceProvider.GetService<ITestHub>();
await testHub.ConnectAsync();

var dbInitializer = scope.ServiceProvider.GetService<DataInitializer>();

await dbInitializer.Initialize();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();

