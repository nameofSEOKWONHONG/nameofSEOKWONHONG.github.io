using MudBlazor;

namespace MudExample.Data;

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

