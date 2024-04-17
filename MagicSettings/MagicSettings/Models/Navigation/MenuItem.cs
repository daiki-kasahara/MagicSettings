namespace MagicSettings.Models.Navigation;

internal class MenuItem(string name, string icon, Tag tag)
{
    public string Name { get; } = name;

    public string Icon { get; } = icon;

    public Tag Tag { get; } = tag;
}
