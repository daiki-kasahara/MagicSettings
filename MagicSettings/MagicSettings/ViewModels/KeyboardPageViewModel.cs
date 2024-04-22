using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Models;
using Windows.System;

namespace MagicSettings.ViewModels;

internal partial class KeyboardPageViewModel : ObservableObject
{
    public ObservableCollection<KeyBindAction> KeyActions = [];

    [ObservableProperty]
    private bool _isEnabledKeyBinding;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _canExecute = false;

    private readonly IKeyboardService _keyboardService;

    public KeyboardPageViewModel(IKeyboardService service)
    {
        _keyboardService = service;
    }

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

        // Todo: 動作確認用 消す
        KeyActions.Add(new KeyBindAction()
        {
            ActionType = Domains.KeyboardActionType.A,
            IsEnabled = true,
            VirtualKey = VirtualKey.LeftButton,
            ProgramPath = "aaa",
            UrlPath = "bbb"
        });

        CanExecute = true;
    }

    public async Task AddNewActionAsync(KeyBindAction keyBindAction)
    {
        KeyActions?.Add(keyBindAction);
        // Todo: ファイル保存処理
    }

    public async Task UpdateActionAsync(KeyBindAction keyBindAction)
    {
        var target = KeyActions?.FirstOrDefault(x => x.VirtualKey == keyBindAction.VirtualKey);

        if (target is null)
        {
            // Todo: エラー処理
            return;
        }

        target.ActionType = keyBindAction.ActionType;
        target.IsEnabled = keyBindAction.IsEnabled;
        target.ProgramPath = keyBindAction.ProgramPath;
        target.UrlPath = keyBindAction.UrlPath;

        // Todo: ファイル保存処理
    }

    public async Task RemoveActionAsync(VirtualKey key)
    {
        var target = KeyActions?.FirstOrDefault(x => x.VirtualKey == key);
        if (target is null)
            return;

        KeyActions?.Remove(target);

        // Todo: ファイル保存処理
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
