using System;
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

    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    private async void KeyBindingToggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledKeyBindingAsync(toggleSwitch.IsOn);
    }

    private async void AddKeyBindButton_Click(object sender, RoutedEventArgs e)
    {
        var resourceLoader = new ResourceLoader();

        var dialog = new ContentDialog
        {
            Content = App.Provider.GetRequiredService<KeyBindEditor>(),
            CloseButtonText = resourceLoader.GetString("KeyBindDialog_Cancel"),
            PrimaryButtonText = resourceLoader.GetString("KeyBindDialog_Add"),
            PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
            RequestedTheme = this.ActualTheme,
            Title = resourceLoader.GetString("KeyBindDialog_Add_Title"),
            XamlRoot = this.XamlRoot,
        };

        var ret = await dialog.ShowAsync();

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
            UrlPath = keyBindEditor.ViewModel.UrlPath,
        };

        await _viewModel.AddNewActionAsync(newAction);
    }

    private async void UpdateKeyBindButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not SettingsCard settingsCard || settingsCard.DataContext is not KeyBindAction updateAction)
            return;

        var resourceLoader = new ResourceLoader();
        var content = App.Provider.GetRequiredService<KeyBindEditor>();

        content.ViewModel.Action = updateAction.ActionType ?? Domains.KeyboardActionType.StartProgram;
        content.ViewModel.Key = updateAction.VirtualKey;
        content.ViewModel.ProgramPath = updateAction.ProgramPath ?? string.Empty;
        content.ViewModel.UrlPath = updateAction.UrlPath ?? string.Empty;
        content.ViewModel.IsEnabledKeyCustom = false;

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
            UrlPath = keyBindEditor.ViewModel.UrlPath,
        };

        await _viewModel.UpdateActionAsync(newAction);
    }

    private async void RemoveKeyBindButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuFlyoutItem menuFlyoutItem || menuFlyoutItem.DataContext is not KeyBindAction action)
            return;

        await _viewModel.RemoveActionAsync(action.VirtualKey);
    }

    private async void IsEnabledKeyBindItem_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch || toggleSwitch.DataContext is not KeyBindAction action)
            return;

        await _viewModel.UpdateActionEnabledAsync(action.VirtualKey, toggleSwitch.IsOn);
    }

    #region Converter

    bool MultiBoolConverter(bool b1, bool b2) => b1 && b2;

    #endregion
}