using MagicSettings.Domains;

namespace MagicSettings.Repositories.Models.SettingsFile;

public class KeyboardAction
{
    public KeyboardActionType? ActionType { get; set; }

    public bool IsEnabled { get; set; }

    public string? ProgramPath { get; set; }

    public string? Url { get; set; }
}
