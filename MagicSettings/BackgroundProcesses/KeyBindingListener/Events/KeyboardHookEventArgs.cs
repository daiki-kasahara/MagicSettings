namespace KeyBindingListener.Events;

internal class KeyboardHookEventArgs
{
    public Keys Key { get; set; }
    public int RetCode { get; set; } = 0;
}
