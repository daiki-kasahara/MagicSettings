using System;
using System.Collections.Generic;
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
            try
            {
                switch (message.PropertyName)
                {
                    case nameof(this.ViewModel.ProgramPath):
                        {
                            var popup = VisualTreeHelper.GetOpenPopupsForXamlRoot(this.XamlRoot).FirstOrDefault();
                            if (popup is null || popup.Child is not ContentDialog dialog)
                                return;

                            dialog.IsPrimaryButtonEnabled = viewModel.Action != KeyboardActionType.StartProgram || ViewModel.ProgramPath != string.Empty;
                            break;
                        }
                    case nameof(this.ViewModel.UrlPath):
                        {
                            var popup = VisualTreeHelper.GetOpenPopupsForXamlRoot(this.XamlRoot).FirstOrDefault();
                            if (popup is null || popup.Child is not ContentDialog dialog)
                                return;

                            dialog.IsPrimaryButtonEnabled = viewModel.Action != KeyboardActionType.OpenUrl || (viewModel.UrlPath.StartsWith(Http) || viewModel.UrlPath.StartsWith(Https));
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
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

    #region Converter

    private string EnumToStringConverter(KeyboardActionType type) => type.ToDisplayString(new ResourceLoader());

    private Visibility ProgramVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.StartProgram ? Visibility.Visible : Visibility.Collapsed;

    private Visibility UrlVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.OpenUrl ? Visibility.Visible : Visibility.Collapsed;

    private bool ProgramPathToBoolConverter(string path) => path == string.Empty;

    private bool UrlToBoolConverter(string url) => !url.StartsWith(Http) && !url.StartsWith(Https);

    private bool AlreadyExistsConverter(IList<VKeys> keyList, VKeys key) => keyList.Contains(key);

    #endregion
}
