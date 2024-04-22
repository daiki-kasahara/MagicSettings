using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Models;
using Windows.System;

namespace MagicSettings.ViewModels;

internal partial class KeyboardPageViewModel(IKeyboardService service) : ObservableObject
{
    public ObservableCollection<KeyBindAction> KeyActions = [];

    [ObservableProperty]
    private bool _isEnabledKeyBinding;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _canExecute = false;

    private readonly IKeyboardService _keyboardService = service;

    public async Task InitializeAsync()
    {
        CanExecute = false;

        var settings = await _keyboardService.GetKeyBindingSettingsAsync();

        IsEnabledKeyBinding = settings.IsEnabledKeyboardBinding;
        if (settings.KeyboardActions is not null)
        {
            foreach (var item in settings.KeyboardActions)
            {
                KeyActions.Add(new KeyBindAction()
                {
                    VirtualKey = (VirtualKey)item.Key,
                    ActionType = item.Value.ActionType,
                    IsEnabled = item.Value.IsEnabled,
                    ProgramPath = item.Value.ProgramPath,
                    UrlPath = item.Value.UrlPath,
                });
            }
            _ = KeyActions.OrderBy(x => x.VirtualKey.ToString());
        }

        CanExecute = true;
    }

    public async Task AddNewActionAsync(KeyBindAction keyBindAction)
    {
        var ret = await _keyboardService.SetKeyBindingActionAsync((int)keyBindAction.VirtualKey,
            new()
            {
                ActionType = keyBindAction.ActionType,
                IsEnabled = keyBindAction.IsEnabled,
                ProgramPath = keyBindAction.ProgramPath,
                UrlPath = keyBindAction.UrlPath
            });

        if (ret)
        {
            KeyActions?.Add(keyBindAction);
        }
    }

    public async Task UpdateActionAsync(KeyBindAction keyBindAction)
    {
        var target = KeyActions?.FirstOrDefault(x => x.VirtualKey == keyBindAction.VirtualKey);

        if (target is null)
        {
            // Todo: エラー処理
            return;
        }

        var ret = await _keyboardService.SetKeyBindingActionAsync((int)keyBindAction.VirtualKey,
            new()
            {
                ActionType = keyBindAction.ActionType,
                IsEnabled = keyBindAction.IsEnabled,
                ProgramPath = keyBindAction.ProgramPath,
                UrlPath = keyBindAction.UrlPath
            });

        if (ret)
        {
            target.ActionType = keyBindAction.ActionType;
            target.IsEnabled = keyBindAction.IsEnabled;
            target.ProgramPath = keyBindAction.ProgramPath;
            target.UrlPath = keyBindAction.UrlPath;
        }
    }

    public async Task UpdateActionAsync(VirtualKey key, bool isEnabled)
    {
        var target = KeyActions?.FirstOrDefault(x => x.VirtualKey == key);

        if (target is null)
        {
            // Todo: エラー処理
            return;
        }

        var ret = await _keyboardService.SetKeyBindingActionAsync((int)key,
            new()
            {
                ActionType = target.ActionType,
                IsEnabled = isEnabled,
                ProgramPath = target.ProgramPath,
                UrlPath = target.UrlPath
            });

        if (ret)
        {
            target.IsEnabled = isEnabled;
        }
    }

    public async Task RemoveActionAsync(VirtualKey key)
    {
        var target = KeyActions?.FirstOrDefault(x => x.VirtualKey == key);
        if (target is null)
            return;

        if (KeyActions?.Remove(target) is not true)
            return;

        await _keyboardService.DeleteKeyBindingActionAsync((int)key);
    }

    public async Task<bool> SetEnabledKeyBindingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledKeyBinding == value)
            return true;

        CanExecute = false;

        if (!await _keyboardService.SetEnabledKeyBindingAsync(value))
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
