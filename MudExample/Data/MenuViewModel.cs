using Blazor.SubtleCrypto;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudExample.Data;

public class MenuViewModel
{
    public List<Menu> Menus { get; private set; }

    public MenuViewModel(HttpClient client, ILocalizer localizer)
    {
        Menus = new List<Menu>();
        
        Menus.Add(new Menu() { Id=1, Name = localizer["LBL0006"], Icon = Icons.Material.Filled.Home, Href = "/", MenuDirection = MenuDirection.Root});
        Menus.Add(new Menu() { Id=2, Name = localizer["LBL0007"], Icon = Icons.Material.Filled.Numbers, Href = "/counter", MenuDirection = MenuDirection.Normal});
        Menus.Add(new Menu() { Id=3, Name = localizer["LBL0008"], Icon = Icons.Material.Filled.WbSunny, Href = "/weather", MenuDirection = MenuDirection.Normal});
        Menus.Add(new Menu() { Id=4, Name = localizer["LBL0009"], Icon = Icons.Material.Filled.TableView, Href = "/table", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=5, Name = localizer["LBL0010"], Icon = Icons.Material.Filled.ShowChart, Href = "/chart", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=6, Name = localizer["LBL0030"], Icon = Icons.Material.Filled.WorkHistory, Href = "/recent-work", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=7, Name = localizer["LBL0032"], Icon = Icons.Material.Filled.NoteAdd, Href = "/diary", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=8, Name = localizer["LBL0011"], Icon = Icons.Material.Filled.Settings, Href = null, MenuDirection = MenuDirection.Sub, SubMenus = new List<Menu>()
        {
            new Menu() { Id=9, Name = localizer["LBL0012"], Icon = Icons.Material.Filled.People, Href = "/settings/users", IconColor = Color.Success, MenuDirection = MenuDirection.Normal},
            new Menu() { Id=10, Name = localizer["LBL0013"], Icon = Icons.Material.Filled.Security, Href = "/settings/security", IconColor = Color.Info, MenuDirection = MenuDirection.Normal},
        }});        
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