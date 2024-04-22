using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Domains;

namespace MagicSettings.Models;

internal partial class KeyBindAction : ObservableObject
{
    [ObservableProperty]
    private VKeys _virtualKey;

    [ObservableProperty]
    private KeyboardActionType? _actionType;

    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string? _programPath;

    [ObservableProperty]
    private string? _urlPath;

    #region Converter

    public string ActionTextConverter(KeyboardActionType? actionType, string? programPath, string? url)
    {
        return actionType switch
        {
            KeyboardActionType.A => $"{actionType}",
            KeyboardActionType.StartProgram => $"{actionType}{Environment.NewLine}{programPath}",
            KeyboardActionType.OpenUrl => $"{actionType}{Environment.NewLine}{url}",
            _ => $"{actionType}",
        };
    }

    #endregion
}
