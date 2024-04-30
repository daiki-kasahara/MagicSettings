using System.Windows;
using KeyBindingListener.Helpers;
using KeyBindingListener.Services;
using MagicSettings.Repositories.Contracts;
using ProcessManager;
using ProcessManager.PipeMessage;

namespace KeyBindingListener;

public partial class MainWindow : Window
{
    private readonly ServerPipe _serverPipe = new(MyProcesses.KeyBindingListener);
    private readonly KeyboardHookHelper _keyboardHookHelper;
    private readonly KeyHookService _keyHookService;
    private readonly IKeyboardBindingRepository _keyboardBindingRepository;

    public MainWindow(KeyboardHookHelper keyboardHookHelper, KeyHookService keyHookService, IKeyboardBindingRepository keyboardBindingRepository)
    {
        InitializeComponent();
        _keyboardHookHelper = keyboardHookHelper;
        _keyHookService = keyHookService;
        _keyboardBindingRepository = keyboardBindingRepository;
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
        // キーフックの終了
        _keyboardHookHelper.UnHook();

        // パイプを閉じる
        _serverPipe.ClosePipe();

        // 終了時は設定を無効にする
        if (App.Mutex is not null)
        {
            Task.Run(() => _keyboardBindingRepository.SaveAsync(false)).Wait();
        }
    }
}