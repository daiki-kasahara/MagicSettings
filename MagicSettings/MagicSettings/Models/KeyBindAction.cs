using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Domains;
using MagicSettings.Extensions;
using Microsoft.Windows.ApplicationModel.Resources;

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
        if (actionType is null)
            return string.Empty;

        var actionString = ((KeyboardActionType)actionType).ToDisplayString(new ResourceLoader());

        return actionType switch
        {
            KeyboardActionType.StartProgram => $"{actionString}{Environment.NewLine}{programPath}",
            KeyboardActionType.OpenUrl => $"{actionString}{Environment.NewLine}{url}",
            _ => $"{actionString}",
        };
    }

    #endregion
}
