using eXtensionSharp;
using MudBlazor;

namespace MudExample.Data;

public enum MenuDirection
{
    Root,
    Normal,
    Sub,
}
public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string? Href { get; set; }
    public MenuDirection MenuDirection { get; set; }
    public Color IconColor { get; set; } = Color.Default;

    public List<Menu> SubMenus { get; set; } = new();
}

public class MenuViewModel
{
    public List<Menu> Menus { get; private set; }

    public MenuViewModel()
    {
        Menus = new List<Menu>();
    }

    public void Initialize()
    {
        Menus.Add(new Menu() { Id=1, Name = "Home", Icon = Icons.Material.Filled.Home, Href = "/", MenuDirection = MenuDirection.Root});
        Menus.Add(new Menu() { Id=2, Name = "Counter", Icon = Icons.Material.Filled.Numbers, Href = "/counter", MenuDirection = MenuDirection.Normal});
        Menus.Add(new Menu() { Id=3, Name = "Weather", Icon = Icons.Material.Filled.WbSunny, Href = "/weather", MenuDirection = MenuDirection.Normal});
        Menus.Add(new Menu() { Id=4, Name = "Table", Icon = Icons.Material.Filled.TableView, Href = "/table", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=4, Name = "Chart", Icon = Icons.Material.Filled.ShowChart, Href = "/chart", MenuDirection = MenuDirection.Normal });
        Menus.Add(new Menu() { Id=5, Name = "Settings", Icon = Icons.Material.Filled.Settings, Href = null, MenuDirection = MenuDirection.Sub, SubMenus = new List<Menu>()
        {
            new Menu() { Id=6, Name = "Users", Icon = Icons.Material.Filled.People, Href = "/settings/users", IconColor = Color.Success, MenuDirection = MenuDirection.Normal},
            new Menu() { Id=7, Name = "Security", Icon = Icons.Material.Filled.Security, Href = "/settings/security", IconColor = Color.Info, MenuDirection = MenuDirection.Normal},
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