﻿using MagicSettings.Contracts.Repositories;
using MagicSettings.Contracts.Services;
using MagicSettings.Repositories;
using MagicSettings.Services;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

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
        m_window.Activate();
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddTransient<MainWindow>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<IThemeService, ThemeService>();
        services.AddTransient<IThemeRepository, ThemeRepository>();

        return services.BuildServiceProvider();
    }
}
