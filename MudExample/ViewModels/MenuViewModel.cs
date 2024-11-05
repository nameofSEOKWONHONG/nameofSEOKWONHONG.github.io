using System.Net.Http.Json;
using Blazor.SubtleCrypto;
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