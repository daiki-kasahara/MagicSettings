using MagicSettings.Contracts.Services;
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
    private readonly IThemeService _themeService;

    public MainWindow(MainWindowViewModel viewModel, IThemeService themeService)
    {
        this.InitializeComponent();

        _viewModel = viewModel;
        _themeService = themeService;

        var loader = new ResourceLoader();
        Title = loader.GetString("Window_Title");
    }

    private void NavigationView_ItemInvoked(NavigationView _, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }

        if (args.InvokedItemContainer.Tag is not Tag tag)
            return;

        switch (tag)
        {
            case Tag.Keyboard:
                ContentFrame.Navigate(typeof(KeyboardPage));
                break;
            case Tag.Screen:
                ContentFrame.Navigate(typeof(ScreenPage));
                break;
            default:
                return;
        }
    }

    private async void MainRootLoadedAsync(object _, RoutedEventArgs __)
    {
        var theme = await _themeService.GetCurrentThemeAsync();

        var requestedTheme = theme switch
        {
            AppTheme.Dark => ElementTheme.Dark,
            AppTheme.Light => ElementTheme.Light,
            AppTheme.System => ElementTheme.Default,
            _ => ElementTheme.Default,
        };

        WindowHelper.RootTheme = requestedTheme;

        ContentFrame.Navigate(typeof(KeyboardPage));
        NavView.SelectedItem = _viewModel.NavigationMenuItems[0];
    }
}
