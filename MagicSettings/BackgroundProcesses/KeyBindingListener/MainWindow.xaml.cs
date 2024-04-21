using System.Windows;
using KeyBindingListener.Helpers;
using KeyBindingListener.Services;

namespace KeyBindingListener;

public partial class MainWindow : Window
{
    private readonly KeyboardHookHelper _keyboardHookHelper = new();
    private readonly KeyHookService _keyHookService = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _keyboardHookHelper.OnKeyDown += _keyHookService.OnKeyDown;
        _keyboardHookHelper.OnKeyUp += _keyHookService.OnKeyUp;
        _keyboardHookHelper.Hook();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        _keyboardHookHelper.UnHook();
    }
}