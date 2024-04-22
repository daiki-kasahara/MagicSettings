using System;
using System.Linq;
using MagicSettings.Domains;
using MagicSettings.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Pickers;
using Windows.System;

namespace MagicSettings.Views.Dialogs;

internal sealed partial class KeyBindEditor : UserControl
{
    public KeyBindEditorViewModel ViewModel { get; }

    public KeyBindEditor(KeyBindEditorViewModel viewModel)
    {
        this.InitializeComponent();
        this.ViewModel = viewModel;
        this.ActionComboBox.ItemsSource = Enum.GetValues(typeof(KeyboardActionType)).Cast<KeyboardActionType>();
    }

    private void KeyInputKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key is VirtualKey.LeftWindows or VirtualKey.RightWindows or VirtualKey.LeftMenu or VirtualKey.RightMenu)
            return;

        ViewModel.Key = e.Key;
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

    #region Converter

    private Visibility ProgramVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.StartProgram ? Visibility.Visible : Visibility.Collapsed;

    private Visibility UrlVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.OpenUrl ? Visibility.Visible : Visibility.Collapsed;

    #endregion
}
