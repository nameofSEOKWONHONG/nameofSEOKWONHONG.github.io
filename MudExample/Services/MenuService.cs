using System.Net.Http.Json;
using eXtensionSharp;
using MudBlazor;
using MudExample.Data;

namespace MudExample.Services;

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