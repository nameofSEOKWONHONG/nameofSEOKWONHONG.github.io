using System.Net.Http.Json;
using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExample.Services;

namespace MudExample.Data;

public class MenuViewModel
{
    private readonly IMenuService _menuService;
    public List<Menu> Menus { get; private set; }
    public string? SelectedHref { get; set; }

    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorage;

    public MenuViewModel(IMenuService menuService, NavigationManager navigationManager, ILocalStorageService localStorageService)
    {
        _menuService = menuService;
        _navigationManager = navigationManager;
        _localStorage = localStorageService;
    }

    public async Task InitializeAsync()
    {
        this.Menus = await _menuService.GetMenuListAsync();
        var href = await _localStorage.GetItemAsync<string>("href");
        this.SelectedHref = href;
    }

    public IEnumerable<Menu> GetAllMenus()
    {
        List<Menu> menus = new();
        foreach (var item in this.Menus.OrderBy(m => m.Id))
        {
            if(item.MenuDirection == MenuDirection.Normal ||
               item.MenuDirection == MenuDirection.Root)
            menus.Add(item);

            if (item.SubMenus.xIsNotEmpty())
            {
                foreach (var subItem in item.SubMenus)
                {
                    if(subItem.MenuDirection == MenuDirection.Normal)
                        menus.Add(subItem);
                }
            }
        }

        return menus;
    }

    public async Task NavigateTo(string href)
    {
        this.SelectedHref = href;
        _navigationManager.NavigateTo(href);
        await _localStorage.SetItemAsync(nameof(href), href);
    }
}