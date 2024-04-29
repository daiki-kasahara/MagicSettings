using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MagicSettings.Domains;
using MagicSettings.Extensions;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.ViewModels;

[ObservableRecipient]
internal partial class KeyBindEditorViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private VKeys _key = VKeys.A;

    [ObservableProperty]
    private KeyboardActionType _action;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _programPath = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private string _urlPath = string.Empty;

    [ObservableProperty]
    private bool _isEnabledKeyCustom = true;

    [ObservableProperty]
    private List<VKeys> _keyList = [];

    public Dictionary<KeyboardActionType, string> KeyboardActions;

    public KeyBindEditorViewModel()
    {
        var resourceLoader = new ResourceLoader();
        KeyboardActions = Enum.GetValues(typeof(KeyboardActionType)).Cast<KeyboardActionType>().ToDictionary(x => x, x => x.ToDisplayString(resourceLoader));
        Messenger = WeakReferenceMessenger.Default;
    }
}
