using MagicSettings.Domains;

namespace KeyBindingListener.Contracts;

/// <summary>
/// キーボードアクションサービス
/// </summary>
public interface IKeyboardActionService
{
    /// <summary>
    /// アクションを実行する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task ActionAsync(VKeys key);
}
