﻿using MagicT.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MagicT.Web.Shared;

public partial class MainLayout
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [CascadingParameter(Name = nameof(SignOutFunc))]
    public Func<Task> SignOutFunc { get; set; }

    [CascadingParameter(Name =nameof(LoginData))]
    public LoginRequest LoginData { get; set; }
    bool _drawerOpen = true;

    [CascadingParameter(Name = nameof(IsDarkMode))]
    public bool IsDarkMode { get; set; }

    [CascadingParameter(Name = nameof(ThemeToggled))]
    public Action ThemeToggled { get; set; }

    public MudTheme CurrentTheme { get; set; } = new();

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    async Task SignOutAsync()
    {
        await SignOutFunc.Invoke();

        NavigationManager.NavigateTo("/");
    }

    public void OnThemeToggled()
    {
        ThemeToggled.Invoke();
    }
}

