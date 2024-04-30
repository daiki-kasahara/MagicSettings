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

        // ViewModelの状態に応じてボタンの有効無効を切り替える
        WeakReferenceMessenger.Default.Register<KeyBindEditor, PropertyChangedMessage<string>>(this, (recipient, message) =>
        {
            switch (message.PropertyName)
            {
                case nameof(this.ViewModel.ProgramPath):
                case nameof(this.ViewModel.Url):
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

    /// <summary>
    /// キー入力の設定
    /// </summary>
    /// <param name="_"></param>
    /// <param name="e"></param>
    private void KeyInputKeyDown(object _, KeyRoutedEventArgs e)
    {
        // 定義されていないキーや特定のキーは設定しない
        if (!Enum.IsDefined(typeof(VKeys), (int)e.Key) ||
            e.Key is VirtualKey.LeftWindows or VirtualKey.RightWindows or VirtualKey.LeftMenu or VirtualKey.RightMenu or VirtualKey.Menu)
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

        // ファイルピッカーを開く
        var file = await openPicker.PickSingleFileAsync();

        // キャンセルの場合何もしない
        if (file is null)
            return;

        ViewModel.ProgramPath = file.Path;
    }

    /// <summary>
    /// アクションの設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs _)
    {
        if (sender is not ComboBox comboBox || comboBox.SelectedItem is not string selectedItem)
            return;

        ViewModel.Action = ViewModel.KeyboardActions.First(x => x.Value == selectedItem).Key;
    }

    /// <summary>
    /// プログラムパスの設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private void ProgramPath_Changed(object sender, TextChangedEventArgs _)
    {
        if (sender is not TextBox textBox)
            return;

        ViewModel.ProgramPath = textBox.Text;
    }

    /// <summary>
    /// Urlの設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private void Url_Changed(object sender, TextChangedEventArgs _)
    {
        if (sender is not TextBox textBox)
            return;

        ViewModel.Url = textBox.Text;
    }

    /// <summary>
    /// ダイアログのボタンの有効無効状態を更新
    /// </summary>
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
                        dialog.IsPrimaryButtonEnabled = (!ViewModel.KeyList.Contains(ViewModel.Key) || !ViewModel.IsEnabledKeyCustom) && !IsInvalidUrl(ViewModel.Url);

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
