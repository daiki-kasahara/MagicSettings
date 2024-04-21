using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Repositories.Models.SettingsFile;
using Windows.System;

namespace MagicSettings.ViewModels;

internal partial class KeyboardPageViewModel : ObservableObject
{
    public ObservableCollection<(string, KeyboardAction)>? KeyActions;

    [ObservableProperty]
    private bool _isEnabledKeyBinding;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _canExecute = false;

    private readonly IKeyboardService _screenService;

    public KeyboardPageViewModel(IKeyboardService service)
    {
        _screenService = service;
    }

    public async Task InitializeAsync()
    {
        CanExecute = false;

        var settings = await _screenService.GetKeyBindingSettingsAsync();

        IsEnabledKeyBinding = settings.IsEnabledKeyboardBinding;
        if (settings.KeyboardActions is not null)
        {
            KeyActions = new ObservableCollection<(string, KeyboardAction)>(
                settings.KeyboardActions
                .Select(x => (((VirtualKey)x.Key).ToString(), new KeyboardAction()))
                .OrderBy(x => x.Item1));
        }

        CanExecute = true;
    }

    public async Task<bool> SetEnabledKeyBindingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledKeyBinding == value)
            return true;

        CanExecute = false;

        if (!await _screenService.SetEnabledKeyBindingAsync(value))
        {
            HasError = true;
            CanExecute = true;
            OnPropertyChanged(nameof(IsEnabledKeyBinding));
            return false;
        }

        HasError = false;
        CanExecute = true;
        IsEnabledKeyBinding = value;
        return true;
    }
}
