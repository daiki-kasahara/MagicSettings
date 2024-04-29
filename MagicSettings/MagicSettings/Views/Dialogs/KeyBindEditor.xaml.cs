using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MagicSettings.Domains;
using MagicSettings.Extensions;
using MagicSettings.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using Windows.System;

namespace MagicSettings.Views.Dialogs;

internal sealed partial class KeyBindEditor : UserControl
{
    private static readonly string Http = "http:";
    private static readonly string Https = "https:";

    public KeyBindEditorViewModel ViewModel { get; }

    public KeyBindEditor(KeyBindEditorViewModel viewModel)
    {
        this.InitializeComponent();
        this.ViewModel = viewModel;
        this.ActionComboBox.ItemsSource = ViewModel.KeyboardActions.Values;

        WeakReferenceMessenger.Default.Register<KeyBindEditor, PropertyChangedMessage<string>>(this, (recipient, message) =>
        {
            switch (message.PropertyName)
            {
                case nameof(this.ViewModel.ProgramPath):
                case nameof(this.ViewModel.UrlPath):
                    {
                        UpdatePrimaryButton();
                        break;
                    }
                default:
                    break;
            }
        });

        WeakReferenceMessenger.Default.Register<KeyBindEditor, PropertyChangedMessage<VKeys>>(this, (recipient, message) =>
        {
            UpdatePrimaryButton();
        });

        WeakReferenceMessenger.Default.Register<KeyBindEditor, PropertyChangedMessage<KeyboardActionType>>(this, (recipient, message) =>
        {
            UpdatePrimaryButton();
        });
    }

    private void KeyInputKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (!Enum.IsDefined(typeof(VKeys), (int)e.Key) ||
            e.Key is VirtualKey.LeftWindows or VirtualKey.RightWindows or VirtualKey.LeftMenu or VirtualKey.RightMenu)
            return;

        ViewModel.Key = (VKeys)e.Key;
    }

    private async void PickAFileButton_Click(object sender, RoutedEventArgs e)
    {
        var openPicker = new FileOpenPicker();

        var hWnd = App.GetWindowHandle();

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add("*");

        var file = await openPicker.PickSingleFileAsync();
        if (file is null)
            return;

        ViewModel.ProgramPath = file.Path;
    }

    private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox || comboBox.SelectedItem is not string selectedItem)
            return;

        ViewModel.Action = ViewModel.KeyboardActions.First(x => x.Value == selectedItem).Key;
    }

    private void ProgramPath_Changed(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        ViewModel.ProgramPath = textBox.Text;
    }

    private void Url_Changed(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        ViewModel.UrlPath = textBox.Text;
    }

    private void UpdatePrimaryButton()
    {
        try
        {
            var popup = VisualTreeHelper.GetOpenPopupsForXamlRoot(this.XamlRoot).FirstOrDefault(x => x.Child is ContentDialog);
            if (popup is null || popup.Child is not ContentDialog dialog)
                return;

            switch (ViewModel.Action)
            {
                case KeyboardActionType.StartProgram:
                    {
                        // StartProgramの場合、キーが重複しておらず path が空文字列ではないときのみ設定可能
                        // 更新の場合は、pathのチェックのみ
                        dialog.IsPrimaryButtonEnabled = (!ViewModel.KeyList.Contains(ViewModel.Key) || !ViewModel.IsEnabledKeyCustom) && !IsFileNotExists(ViewModel.ProgramPath);

                        break;
                    }
                case KeyboardActionType.OpenUrl:
                    {
                        // OpenUrlの場合、キーが重複しておらず url が特定の文字列で始まっているときのみ設定可能
                        // 更新の場合は、urlのチェックのみ
                        dialog.IsPrimaryButtonEnabled = (!ViewModel.KeyList.Contains(ViewModel.Key) || !ViewModel.IsEnabledKeyCustom) && !IsInvalidUrl(ViewModel.UrlPath);

                        break;
                    }
                default:
                    {
                        // その他のアクションの場合、キーが重複していないときのみ設定可能
                        // 更新の場合は、無条件で設定可能
                        dialog.IsPrimaryButtonEnabled = (!ViewModel.KeyList.Contains(ViewModel.Key) || !ViewModel.IsEnabledKeyCustom);

                        break;
                    }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    #region Converter

    private string EnumToStringConverter(KeyboardActionType type) => type.ToDisplayString(new ResourceLoader());

    private Visibility ProgramVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.StartProgram ? Visibility.Visible : Visibility.Collapsed;

    private Visibility UrlVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.OpenUrl ? Visibility.Visible : Visibility.Collapsed;

    private bool IsFileNotExists(string path) => !File.Exists(path);

    private bool IsInvalidUrl(string url) => !url.StartsWith(Http) && !url.StartsWith(Https);

    private bool AlreadyExistsConverter(IList<VKeys> keyList, VKeys key) => keyList.Contains(key);

    #endregion
}
