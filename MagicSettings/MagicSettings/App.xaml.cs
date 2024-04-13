using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace MagicSettings;

/// <summary>
/// アプリケーションクラス
/// </summary>
public partial class App : Application
{
    public static ServiceProvider Provider { get; } = GetServiceProvider();
    private Window? m_window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = Provider.GetRequiredService<MainWindow>();
        m_window.Activate();
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddTransient<MainWindow>();
        services.AddTransient<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}
