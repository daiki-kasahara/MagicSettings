using MagicSettings.Domains;
using Windows.System;

namespace MagicSettings.Models;

internal class KeyBindAction
{
    public VirtualKey VirtualKey { get; set; }

    public KeyboardActionType? ActionType { get; set; }

    public bool IsEnabled { get; set; }

    public string? ProgramPath { get; set; }

    public string? UrlPath { get; set; }
}
