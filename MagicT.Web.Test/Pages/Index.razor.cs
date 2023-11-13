﻿using MagicT.Shared.Models;
using MessagePipe;
using Microsoft.AspNetCore.Components;

namespace MagicT.Web.Test.Pages;

public partial class Index
{
    [Inject]
    IRemoteRequestHandler<int, string> remoteHandler { get; set; }

    [Inject]
    IDistributedPublisher<string, USERS> publisher { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await A(remoteHandler);
        await P(publisher);
    }
    // client

 
    async Task A(IRemoteRequestHandler<int, string> remoteHandler)
    {
        var v = await remoteHandler.InvokeAsync(9999);
        Console.WriteLine(v); // ECHO:9999
    }

 
    public async Task P(IDistributedPublisher<string, USERS> publisher)
    {
        // publish value to remote process.
        await publisher.PublishAsync("foobar", new USERS { UB_FULLNAME = "Licentia"});
    }
}
