using MagicSettings.Domains;

namespace MagicSettings.Repositories.Models.SettingsFile;

public class KeyboardBindingSettings
{
    public bool IsEnabledKeyboardBinding { get; set; } = false;

    public Dictionary<int, KeyboardAction>? KeyboardActions { get; set; }
}

public class KeyboardAction
{
    public KeyboardActionType? ActionType { get; set; }

    public bool IsEnabled { get; set; }

    public string? ProgramPath { get; set; }

    public string? UrlPath { get; set; }
}
