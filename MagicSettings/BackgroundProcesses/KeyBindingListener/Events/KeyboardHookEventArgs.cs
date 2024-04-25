using MagicSettings.Domains;

namespace KeyBindingListener.Events;

/// <summary>
/// キーボードフックイベントの引数
/// </summary>
public class KeyboardHookEventArgs
{
    public VKeys Key { get; set; }
    public int RetCode { get; set; } = 0;
}
