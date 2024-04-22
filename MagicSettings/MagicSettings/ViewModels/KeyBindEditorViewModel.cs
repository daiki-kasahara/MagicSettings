using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Domains;
using Windows.System;

namespace MagicSettings.ViewModels;

internal partial class KeyBindEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private VirtualKey _key;

    [ObservableProperty]
    private KeyboardActionType _action;

    [ObservableProperty]
    private string _programPath = string.Empty;

    [ObservableProperty]
    private string _urlPath = string.Empty;

    [ObservableProperty]
    private bool _isEnabledKeyCustom = true;
}
