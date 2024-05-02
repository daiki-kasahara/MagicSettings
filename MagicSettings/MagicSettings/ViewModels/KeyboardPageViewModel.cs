using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models;

namespace MagicSettings.ViewModels;

internal partial class KeyboardPageViewModel(IKeyboardService service) : ObservableObject
{
    public ObservableCollection<KeyBindAction> KeyActions = [];

    [ObservableProperty]
    private bool _isEnabledKeyBinding;

    [ObservableProperty]
    private bool _enabledError = false;

    [ObservableProperty]
    private bool _keyBindError = false;

    [ObservableProperty]
    private bool _canExecute = false;

    private readonly IKeyboardService _keyboardService = service;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
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
                    VirtualKey = (VKeys)item.Key,
                    ActionType = item.Value.ActionType,
                    IsEnabled = item.Value.IsEnabled,
                    ProgramPath = item.Value.ProgramPath,
                    Url = item.Value.Url,
                });
            }
        }

        InsertionSort(KeyActions);
        CanExecute = true;
    }

    /// <summary>
    /// キーバインディングの有効無効を設定する
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<bool> SetEnabledKeyBindingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledKeyBinding == value)
            return true;

        CanExecute = false;

        if (!await _keyboardService.SetEnabledKeyBindingAsync(value))
        {
            EnabledError = true;
            CanExecute = true;
            OnPropertyChanged(nameof(IsEnabledKeyBinding));
            return false;
        }

        EnabledError = false;
        CanExecute = true;
        IsEnabledKeyBinding = value;
        return true;
    }

    /// <summary>
    /// 新しいキーバインディングを追加する
    /// </summary>
    /// <param name="keyBindAction"></param>
    /// <returns></returns>
    public async Task AddNewKeyBindingAsync(KeyBindAction keyBindAction)
    {
        // すでにキーが登録されている場合はエラー
        if (KeyActions.Count(x => x.VirtualKey == keyBindAction.VirtualKey) is 1)
        {
            KeyBindError = true;
            return;
        }

        CanExecute = false;

        var isSucceeded = await _keyboardService.SetKeyBindingActionAsync((int)keyBindAction.VirtualKey,
            new()
            {
                ActionType = keyBindAction.ActionType,
                IsEnabled = keyBindAction.IsEnabled,
                ProgramPath = keyBindAction.ProgramPath,
                Url = keyBindAction.Url
            });

        if (isSucceeded)
        {
            KeyActions.Add(keyBindAction);
            InsertionSort(KeyActions);
            KeyBindError = false;
        }
        else
        {
            KeyBindError = true;
        }

        CanExecute = true;
    }

    /// <summary>
    /// キーバインディングの更新する
    /// </summary>
    /// <param name="keyBindAction"></param>
    /// <returns></returns>
    public async Task UpdateActionAsync(KeyBindAction keyBindAction)
    {
        var target = KeyActions.FirstOrDefault(x => x.VirtualKey == keyBindAction.VirtualKey);

        // 更新するキーが存在しない場合はエラー
        if (target is null)
        {
            KeyBindError = true;
            return;
        }

        CanExecute = false;

        var isSucceeded = await _keyboardService.SetKeyBindingActionAsync((int)keyBindAction.VirtualKey,
            new()
            {
                ActionType = keyBindAction.ActionType,
                IsEnabled = keyBindAction.IsEnabled,
                ProgramPath = keyBindAction.ProgramPath,
                Url = keyBindAction.Url
            });

        if (isSucceeded)
        {
            target.ActionType = keyBindAction.ActionType;
            target.IsEnabled = keyBindAction.IsEnabled;
            target.ProgramPath = keyBindAction.ProgramPath;
            target.Url = keyBindAction.Url;

            KeyBindError = false;
        }
        else
        {
            KeyBindError = true;
        }

        CanExecute = true;

    }

    /// <summary>
    /// 各キーバインディングの有効無効を設定する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public async Task UpdateActionEnabledAsync(VKeys key, bool isEnabled)
    {
        var target = KeyActions.FirstOrDefault(x => x.VirtualKey == key);

        // 更新するキーが存在しない場合はエラー
        if (target is null)
        {
            KeyBindError = true;
            return;
        }

        // 設定する値が現在と同じ場合何もせず成功を返す
        if (target.IsEnabled == isEnabled)
            return;

        CanExecute = false;

        var isSucceeded = await _keyboardService.SetKeyBindingActionAsync((int)key,
            new()
            {
                ActionType = target.ActionType,
                IsEnabled = isEnabled,
                ProgramPath = target.ProgramPath,
                Url = target.Url
            });

        if (isSucceeded)
        {
            target.IsEnabled = isEnabled;

            KeyBindError = false;
        }
        else
        {
            KeyBindError = true;
        }

        CanExecute = true;
    }

    /// <summary>
    /// 登録されているキーを削除する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task RemoveActionAsync(VKeys key)
    {
        var target = KeyActions.FirstOrDefault(x => x.VirtualKey == key);

        // 削除対象のキーが存在しない場合はエラー
        if (target is null)
        {
            KeyBindError = true;
            return;
        }

        CanExecute = false;

        var isSucceeded = await _keyboardService.DeleteKeyBindingActionAsync((int)key);

        if (isSucceeded)
        {
            KeyBindError = KeyActions.Remove(target) is not true;
        }
        else
        {
            KeyBindError = true;
        }

        CanExecute = true;
    }

    /// <summary>
    /// 仮想キーコード順に並び替える
    /// </summary>
    /// <param name="list">仮想キー</param>
    private static void InsertionSort(IList<KeyBindAction> list)
    {
        for (var i = 1; i < list.Count; i++)
        {
            var currentValue = list[i];
            var j = i - 1;
            while (j >= 0 && (int)list[j].VirtualKey > (int)currentValue.VirtualKey)
            {
                list[j + 1] = list[j];
                j--;
            }
            list[j + 1] = currentValue;
        }
    }
}
