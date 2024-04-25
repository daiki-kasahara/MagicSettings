using MagicSettings.Domains;

namespace KeyBindingListener.Contracts;

/// <summary>
/// アクションインスタンスを生成するファクトリ
/// </summary>
public interface IActionFactory
{
    public IAction Create(KeyboardActionType actionType, string fileName);
}
