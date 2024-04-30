using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Controls;
using MagicSettings.Models;
using MagicSettings.ViewModels;
using MagicSettings.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.Views;

public sealed partial class KeyboardPage : Page
{
    private readonly KeyboardPageViewModel _viewModel;

    public KeyboardPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<KeyboardPageViewModel>();
    }

    /// <summary>
    /// �y�[�W���\�����ꂽ�Ƃ��Ɏ��s���鏈��
    /// </summary>
    /// <param name="_"></param>
    /// <param name="__"></param>
    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    /// <summary>
    /// �L�[�o�C���f�B���O�̗L�������ݒ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void KeyBindingToggled(object sender, RoutedEventArgs _)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledKeyBindingAsync(toggleSwitch.IsOn);
    }

    /// <summary>
    /// �L�[�o�C���f�B���O�̒ǉ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void AddKeyBindButton_Click(object sender, RoutedEventArgs e)
    {
        // �L�[�ݒ�_�C�A���O��\��
        var resourceLoader = new ResourceLoader();
        var content = App.Provider.GetRequiredService<KeyBindEditor>();
        content.ViewModel.KeyList = _viewModel.KeyActions.Select(x => x.VirtualKey).ToList();

        var dialog = new ContentDialog
        {
            Content = content,
            CloseButtonText = resourceLoader.GetString("KeyBindDialog_Cancel"),
            IsPrimaryButtonEnabled = false,
            PrimaryButtonText = resourceLoader.GetString("KeyBindDialog_Add"),
            PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
            RequestedTheme = this.ActualTheme,
            Title = resourceLoader.GetString("KeyBindDialog_Add_Title"),
            XamlRoot = this.XamlRoot,
        };

        var ret = await dialog.ShowAsync();

        // �L�����Z���̎��͉������Ȃ�
        if (ret != ContentDialogResult.Primary)
            return;

        if (dialog.Content is not KeyBindEditor keyBindEditor)
            return;

        var newAction = new KeyBindAction()
        {
            ActionType = keyBindEditor.ViewModel.Action,
            IsEnabled = true,
            ProgramPath = keyBindEditor.ViewModel.ProgramPath,
            Url = keyBindEditor.ViewModel.Url,
            VirtualKey = keyBindEditor.ViewModel.Key,
        };

        await _viewModel.AddNewKeyBindingAsync(newAction);
    }

    /// <summary>
    /// �A�C�e���̃J�[�h���N���b�N�������̏���
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void UpdateKeyBindButton_Click(object sender, RoutedEventArgs _)
    {
        if (sender is not SettingsCard settingsCard || settingsCard.DataContext is not KeyBindAction updateAction)
            return;

        await UpdateItemAsync(updateAction);
    }

    /// <summary>
    /// �ҏW�{�^�����N���b�N�������̏���
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void EditMenuItemButton_Click(object sender, RoutedEventArgs _)
    {
        if (sender is not MenuFlyoutItem settingsCard || settingsCard.DataContext is not KeyBindAction updateAction)
            return;

        await UpdateItemAsync(updateAction);
    }

    /// <summary>
    /// �폜�{�^�������������̏���
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void RemoveKeyBindButton_Click(object sender, RoutedEventArgs _)
    {
        if (sender is not MenuFlyoutItem menuFlyoutItem || menuFlyoutItem.DataContext is not KeyBindAction action)
            return;

        await _viewModel.RemoveActionAsync(action.VirtualKey);
    }

    /// <summary>
    /// �e�L�[�o�C���f�B���O�̗L�������ݒ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void IsEnabledKeyBindItem_Toggled(object sender, RoutedEventArgs _)
    {
        if (sender is not ToggleSwitch toggleSwitch || toggleSwitch.DataContext is not KeyBindAction action)
            return;

        await _viewModel.UpdateActionEnabledAsync(action.VirtualKey, toggleSwitch.IsOn);
    }

    /// <summary>
    /// �����L�[�o�C���f�B���O�̍X�V
    /// </summary>
    /// <param name="updateAction"></param>
    /// <returns></returns>
    private async Task UpdateItemAsync(KeyBindAction updateAction)
    {
        // �L�[�ݒ�_�C�A���O��\��
        var resourceLoader = new ResourceLoader();
        var content = App.Provider.GetRequiredService<KeyBindEditor>();

        content.ViewModel.Action = updateAction.ActionType ?? Domains.KeyboardActionType.StartProgram;
        content.ViewModel.IsEnabledKeyCustom = false;
        content.ViewModel.Key = updateAction.VirtualKey;
        content.ViewModel.ProgramPath = updateAction.ProgramPath ?? string.Empty;
        content.ViewModel.Url = updateAction.Url ?? string.Empty;

        var dialog = new ContentDialog
        {
            Content = content,
            CloseButtonText = resourceLoader.GetString("KeyBindDialog_Cancel"),
            PrimaryButtonText = resourceLoader.GetString("KeyBindDialog_Update"),
            PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
            RequestedTheme = this.ActualTheme,
            Title = resourceLoader.GetString("KeyBindDialog_Update_Title"),
            XamlRoot = this.XamlRoot,
        };

        var ret = await dialog.ShowAsync();

        // �L�����Z���̎��͉������Ȃ�
        if (ret != ContentDialogResult.Primary)
            return;

        if (dialog.Content is not KeyBindEditor keyBindEditor)
            return;

        var newAction = new KeyBindAction()
        {
            ActionType = keyBindEditor.ViewModel.Action,
            IsEnabled = true,
            VirtualKey = keyBindEditor.ViewModel.Key,
            ProgramPath = keyBindEditor.ViewModel.ProgramPath,
            Url = keyBindEditor.ViewModel.Url,
        };

        await _viewModel.UpdateActionAsync(newAction);
    }

    #region Converter

    private bool MultiBoolConverter(bool b1, bool b2) => b1 && b2;

    #endregion
}
