using MagicSettings.Domains;

namespace KeyBindingListener.Contracts;

/// <summary>
/// キーボードアクションサービス
/// </summary>
public interface IKeyboardActionService
{
    public Task ActionAsync(VKeys key);
}
