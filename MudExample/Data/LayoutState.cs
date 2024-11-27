using Blazored.LocalStorage;
using eXtensionSharp;

namespace MudExample.Data;

public class LayoutState
{
    public bool DrawerOpen { get; set; }
    public bool IsDark { get; set; }
    public string Culture { get; private set; }

    private readonly ILocalStorageService _localStorage;
    public LayoutState(ILocalStorageService localStorageService)
    {
        _localStorage = localStorageService;
    }

    public async Task InitializeAsync()
    {
        IsDark = await _localStorage.GetItemAsync<bool>(nameof(LayoutState.IsDark));
        DrawerOpen = await _localStorage.GetItemAsync<bool>(nameof(LayoutState.DrawerOpen));
    }

    public async Task SetDrawerOpen(bool isDrawerOpen)
    {
        this.DrawerOpen = isDrawerOpen;
        await _localStorage.SetItemAsync(nameof(LayoutState.DrawerOpen), isDrawerOpen);
    }
    
    public async Task SetIsDark(bool isDark)
    {
        this.IsDark = isDark;
        await _localStorage.SetItemAsync(nameof(LayoutState.IsDark), isDark);
    }

    public async Task SetCulture(string culture)
    {
        this.Culture = culture;
        await _localStorage.SetItemAsync(nameof(LayoutState.Culture), culture);
    }
}