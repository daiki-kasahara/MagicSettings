using MagicSettings.Domains;

namespace KeyBindingListener.Contracts;

/// <summary>
/// アクションインスタンスを生成するファクトリ
/// </summary>
public interface IActionFactory
{
    /// <summary>
    /// 生成する
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public IAction Create(KeyboardActionType actionType, string fileName);
}
