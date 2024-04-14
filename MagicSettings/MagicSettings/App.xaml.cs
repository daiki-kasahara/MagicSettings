using MagicSettings.Contracts.Services;
using MagicSettings.Helper;
using MagicSettings.Repositories;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Services;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using ProcessManager;
using ProcessManager.Contracts;

namespace MagicSettings;

/// <summary>
/// アプリのエントリポイント
/// </summary>
public partial class App : Application
{
    // DIコンテナ
    public static ServiceProvider Provider { get; } = GetServiceProvider();

    // メインウィンドウ
    private static Window? m_window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = Provider.GetRequiredService<MainWindow>();
        WindowHelper.TrackWindow(m_window);
        m_window.Activate();
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        // Add View
        services.AddTransient<MainWindow>();

        // Add View Model
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<ScreenPageViewModel>();

        // Add Model
        services.AddTransient<IThemeService, ThemeService>();
        services.AddTransient<IScreenRepository, ScreenRepository>();
        services.AddTransient<IThemeRepository, ThemeRepository>();
        services.AddTransient<IScreenService, ScreenService>();
        services.AddTransient<IProcessLauncher, ProcessLauncher>();

        return services.BuildServiceProvider();
    }
}
