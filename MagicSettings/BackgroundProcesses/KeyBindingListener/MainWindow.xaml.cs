using System.Windows;
using KeyBindingListener.Helpers;
using KeyBindingListener.Services;
using ProcessManager;
using ProcessManager.PipeMessage;

namespace KeyBindingListener;

public partial class MainWindow : Window
{
    private readonly KeyboardHookHelper _keyboardHookHelper = new();
    private readonly KeyHookService _keyHookService = new();
    private readonly ServerPipe _serverPipe = new(MyProcesses.KeyBindingListener);

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _keyboardHookHelper.OnKeyDown += _keyHookService.OnKeyDown;
        _keyboardHookHelper.OnKeyUp += _keyHookService.OnKeyUp;
        _keyboardHookHelper.Hook();

        _serverPipe.OnAction += (RequestMessage message) =>
        {
            if (message.Cmd == "Terminate")
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    System.Windows.Application.Current.Shutdown();
                }));
            }
        };
        _serverPipe.OpenPipe();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        _keyboardHookHelper.UnHook();

        Task.Run(_serverPipe.ClosePipe).Wait();
    }
}