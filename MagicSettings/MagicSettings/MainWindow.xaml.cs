using MagicSettings.Domains;
using MagicSettings.Helper;
using MagicSettings.Models.Navigation;
using MagicSettings.ViewModels;
using MagicSettings.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings;

/// <summary>
/// メインウィンドウ
/// </summary>
internal sealed partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(MainWindowViewModel viewModel)
    {
        this.InitializeComponent();

        _viewModel = viewModel;

        var loader = new ResourceLoader();
        Title = loader.GetString("Window_Title");
    }

    private void NavigationView_ItemInvoked(NavigationView _, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            // 設定ページに遷移
            ContentFrame.Navigate(typeof(SettingsPage));
        }

        if (args.InvokedItemContainer.Tag is not Tag tag)
            return;

        // 各ページに遷移
        switch (tag)
        {
            case Tag.Keyboard:
                {
                    ContentFrame.Navigate(typeof(KeyboardPage));
                    break;
                }
            case Tag.Screen:
                {
                    ContentFrame.Navigate(typeof(ScreenPage));
                    break;
                }
            default:
                return;
        }
    }

    /// <summary>
    /// ウィンドウが表示されたときに実行する処理
    /// </summary>
    /// <param name="_"></param>
    /// <param name="__"></param>
    private async void MainRootLoadedAsync(object _, RoutedEventArgs __)
    {
        var theme = await _viewModel.GetCurrentThemeAsync();

        // テーマの設定
        WindowHelper.RootTheme = theme switch
        {
            AppTheme.Dark => ElementTheme.Dark,
            AppTheme.Light => ElementTheme.Light,
            AppTheme.System => ElementTheme.Default,
            _ => ElementTheme.Default,
        };

        ContentFrame.Navigate(typeof(KeyboardPage));
        NavView.SelectedItem = _viewModel.NavigationMenuItems[0];
    }
}
