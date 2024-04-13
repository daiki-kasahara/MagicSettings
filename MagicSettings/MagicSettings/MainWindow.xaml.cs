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

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }

        if (args.InvokedItemContainer.Tag is not Tag tag)
            return;

        switch (tag)
        {
            case Tag.Home:
                ContentFrame.Navigate(typeof(HomePage));
                break;
            case Tag.KeyBinding:
                ContentFrame.Navigate(typeof(KeyBindingPage));
                break;
            case Tag.Display:
                ContentFrame.Navigate(typeof(DisplayPage));
                break;
            default:
                return;
        }
    }
}
