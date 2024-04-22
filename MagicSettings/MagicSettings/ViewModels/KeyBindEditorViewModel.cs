using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Domains;

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
}
