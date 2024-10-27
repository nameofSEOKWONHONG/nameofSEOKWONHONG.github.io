using System.Net.Http.Json;
using Blazor.SubtleCrypto;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudExample.Data;

public interface IMenuService
{
    Task<List<Menu>> GetMenuListAsync();
}

public class MenuService : IMenuService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalizer _localizer;

    public MenuService(HttpClient client, ILocalizer localizer)
    {
        _httpClient = client;
        _localizer = localizer;
    }

    public async Task<List<Menu>> GetMenuListAsync()
    {
        var res = await _httpClient.GetAsync($"/data/menu.json");
        res.EnsureSuccessStatusCode();
        
        var result = await res.Content.ReadFromJsonAsync<List<Menu>>();
        foreach (var menu in result)
        {
            if (menu.Name == "LBL0032")
            {
                menu.Icon = Icons.Material.Filled.EditNote;
            }
            menu.Name = _localizer[menu.Name];
            if (menu.SubMenus.xIsNotEmpty())
            {
                foreach (var subMenu in menu.SubMenus)
                {
                    subMenu.Name = _localizer[subMenu.Name];    
                }
            }
        }

        return result;
    }
}

public class MenuViewModel
{
    private readonly IMenuService _menuService;
    public List<Menu> Menus { get; private set; }

    public MenuViewModel(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task InitializeAsync()
    {
        this.Menus = await _menuService.GetMenuListAsync();
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
}