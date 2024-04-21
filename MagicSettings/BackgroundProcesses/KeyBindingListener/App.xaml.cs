using System.Windows;

namespace KeyBindingListener;

public partial class App : System.Windows.Application
{
    private Mutex? _mutex;
    private Window? _window;

    private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        _mutex = new Mutex(false, "MagicSettings.KeyBindingListener");

        if (!_mutex.WaitOne(0, false))
        {
            _mutex.Close();
            _mutex = null;

            this.Shutdown();
        }

        _window = new MainWindow();
        _window.Show();
    }
}
