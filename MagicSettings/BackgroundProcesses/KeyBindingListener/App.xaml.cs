using System.Windows;
using KeyBindingListener.Contracts;
using KeyBindingListener.Factories;
using KeyBindingListener.Helpers;
using KeyBindingListener.Services;
using MagicSettings.Repositories;
using MagicSettings.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBindingListener;

public partial class App : Application
{
    // DIコンテナ
    public static ServiceProvider Provider { get; } = GetServiceProvider();

    public static Mutex? Mutex;

    private Window? _window;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Mutex = new Mutex(false, "MagicSettings.KeyBindingListener");

        if (!Mutex.WaitOne(0, false))
        {
            // すでにプロセスが起動している場合は終了する
            Mutex.Close();
            Mutex = null;

            this.Shutdown();
        }

        _window = Provider.GetRequiredService<MainWindow>();
        _window.Show();
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainWindow>();
        services.AddTransient<KeyHookService>();
        services.AddTransient<KeyboardHookHelper>();
        services.AddTransient<KeyHookService>();
        services.AddTransient<IKeyboardBindingRepository, KeyboardBindingRepository>();
        services.AddTransient<IKeyboardActionService, KeyboardActionService>();
        services.AddTransient<IActionFactory, ActionFactory>();

        return services.BuildServiceProvider();
    }
}
