using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Domains;
using MagicSettings.Extensions;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.ViewModels;

internal partial class KeyBindEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private VKeys _key = VKeys.A;

    [ObservableProperty]
    private KeyboardActionType _action;

    [ObservableProperty]
    private string _programPath = string.Empty;

    [ObservableProperty]
    private string _urlPath = string.Empty;

    [ObservableProperty]
    private bool _isEnabledKeyCustom = true;

    public Dictionary<KeyboardActionType, string> KeyboardActions;

    public KeyBindEditorViewModel()
    {
        var resourceLoader = new ResourceLoader();
        KeyboardActions = Enum.GetValues(typeof(KeyboardActionType)).Cast<KeyboardActionType>().ToDictionary(x => x, x => x.ToDisplayString(resourceLoader));
    }
}
