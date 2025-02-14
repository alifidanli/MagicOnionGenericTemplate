﻿using MagicT.Client.Managers;
using MagicT.Shared.Models.ServiceModels;
using MagicT.Shared.Models.ViewModels;
using MagicT.Shared.Services;
using MessagePipe;
using Microsoft.AspNetCore.Components;

namespace MagicT.WebTemplate.Pages;


public partial class Login
{
    [CascadingParameter(Name = nameof(LoginData))]
    public (string identifier, EncryptedData<string> securePassword) LoginData { get; set; }

    public LoginRequest LoginRequest { get; set; } = new();

    [Inject] public LoginManager LoginManager { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    IAuthenticationService Service { get; set; }

    [Inject]
    public IDistributedSubscriber<string, byte[]> subscriber { get; set; }



    public async Task LoginAsync()
    {
        await ExecuteAsync(async () =>
        {
            var result = await Service.LoginWithUsername(LoginRequest);

            await LoginManager.SignInAsync(LoginRequest);

            await LoginManager.TokenRefreshSubscriber(LoginRequest);

            NavigationManager.NavigateTo("/");

        });
        
    }

    protected override Task OnBeforeInitializeAsync()
    {
        NavigationManager.NavigateTo("/");
        return base.OnBeforeInitializeAsync();
    }
}
