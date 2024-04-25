using System;
using System.Linq;
using MagicSettings.Domains;
using MagicSettings.Extensions;
using MagicSettings.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.ApplicationModel.Resources;
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
        this.ActionComboBox.ItemsSource = ViewModel.KeyboardActions.Values;
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

    #region Converter

    private string EnumToStringConverter(KeyboardActionType type) => type.ToDisplayString(new ResourceLoader());

    private Visibility ProgramVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.StartProgram ? Visibility.Visible : Visibility.Collapsed;

    private Visibility UrlVisibilityConverter(KeyboardActionType type) => type is KeyboardActionType.OpenUrl ? Visibility.Visible : Visibility.Collapsed;

    #endregion
}
