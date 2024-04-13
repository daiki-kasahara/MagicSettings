namespace MagicSettings.Models;

internal class NavigationMenuItem(string name, string icon, string tag)
{
    public string Name { get; set; } = name;

    public string Icon { get; set; } = icon;

    public string Tag { get; set; } = tag;
}
