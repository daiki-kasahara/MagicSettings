using System.Windows;
using KeyBindingListener.Helpers;
using KeyBindingListener.Services;
using ProcessManager;
using ProcessManager.PipeMessage;

namespace KeyBindingListener;

public partial class MainWindow : Window
{
    private readonly KeyboardHookHelper _keyboardHookHelper;
    private readonly KeyHookService _keyHookService;
    private readonly ServerPipe _serverPipe = new(MyProcesses.KeyBindingListener);

    public MainWindow(KeyboardHookHelper keyboardHookHelper, KeyHookService keyHookService)
    {
        InitializeComponent();
        _keyboardHookHelper = keyboardHookHelper;
        _keyHookService = keyHookService;
    }

    /// <summary>
    /// アプリの開始処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _keyboardHookHelper.OnKeyDown += _keyHookService.OnKeyDown;
        _keyboardHookHelper.OnKeyUp += _keyHookService.OnKeyUp;
        _keyboardHookHelper.Hook();

        _serverPipe.OnAction += (RequestMessage message) =>
        {
            if (message.Cmd == "Terminate")
            {
                // メインスレッドでアプリの終了処理を実行する
                Dispatcher.Invoke(new Action(() =>
                {
                    Application.Current.Shutdown();
                }));
            }
        };
        _serverPipe.OpenPipe();
    }

    /// <summary>
    /// アプリの終了処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closed(object sender, EventArgs e)
    {
        _keyboardHookHelper.UnHook();

        _serverPipe.ClosePipe();
    }
}