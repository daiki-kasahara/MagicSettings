using MagicSettings.Domains;

namespace MagicSettings.Models.SettingsFile;

public class ScreenSettings
{
    public bool IsEnabledBlueLightBlocking { get; set; } = false;

    public BlueLightBlocking BlueLightBlocking { get; set; } = BlueLightBlocking.Twenty;
}
