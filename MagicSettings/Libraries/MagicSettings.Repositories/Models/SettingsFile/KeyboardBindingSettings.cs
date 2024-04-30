namespace MagicSettings.Repositories.Models.SettingsFile;

public class KeyboardBindingSettings
{
    public bool IsEnabledKeyboardBinding { get; set; } = false;

    public Dictionary<int, KeyboardAction>? KeyboardActions { get; set; }
}
