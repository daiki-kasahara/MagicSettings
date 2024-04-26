namespace MagicSettings.Repositories.Models;

public class About(string name, string version, string copyright)
{
    public string AppName { get; } = name;
    public string AppVersion { get; } = version;
    public string Copyright { get; } = copyright;
}
