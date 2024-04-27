using System;
using System.Diagnostics;
using MagicSettings.Contracts.Services;
using MagicSettings.Helper;
using MagicSettings.Repositories;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Repositories;
using MagicSettings.Services;
using MagicSettings.ViewModels;
using MagicSettings.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
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
    private static Window? _window;
    private static DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public static nint GetWindowHandle() => WinRT.Interop.WindowNative.GetWindowHandle(_window);

    public App()
    {
        this.InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var mainInstance = AppInstance.FindOrRegisterForKey("MagicSettings");
        if (!mainInstance.IsCurrent)
        {
            var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);

            Process.GetCurrentProcess().Kill();
            return;
        }

        _window = Provider.GetRequiredService<MainWindow>();
        WindowHelper.TrackWindow(_window);
        WindowHelper.SetMinWindowSize(_window);
        _window.Activate();

        mainInstance.Activated += MainInstance_Activated;
    }

    private void MainInstance_Activated(object? sender, AppActivationArguments e)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            _window?.Activate();
            WindowHelper.SetForeground(_window);
        });
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        // Add View
        services.AddTransient<MainWindow>();
        services.AddTransient<KeyBindEditor>();

        // Add View Model
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ScreenPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<KeyboardPageViewModel>();
        services.AddTransient<KeyBindEditorViewModel>();

        // Add Model
        services.AddTransient<IAssemblyInfoRepository, AssemblyInfoRepository>();
        services.AddTransient<IKeyboardBindingRepository, KeyboardBindingRepository>();
        services.AddTransient<IScreenRepository, ScreenRepository>();
        services.AddTransient<IThemeRepository, ThemeRepository>();
        services.AddTransient<IKeyboardService, KeyboardService>();
        services.AddTransient<IScreenService, ScreenService>();
        services.AddTransient<IThemeService, ThemeService>();
        services.AddTransient<IProcessController, ProcessController>();

        return services.BuildServiceProvider();
    }
}
