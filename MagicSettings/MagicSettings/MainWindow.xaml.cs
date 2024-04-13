using MagicSettings.ViewModels;
using Microsoft.UI.Xaml;
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
}
