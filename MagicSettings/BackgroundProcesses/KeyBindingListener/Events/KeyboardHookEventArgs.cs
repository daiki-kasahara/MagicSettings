namespace KeyBindingListener.Events;

/// <summary>
/// キーボードフックイベントの引数
/// </summary>
internal class KeyboardHookEventArgs
{
    public Keys Key { get; set; }
    public int RetCode { get; set; } = 0;
}
