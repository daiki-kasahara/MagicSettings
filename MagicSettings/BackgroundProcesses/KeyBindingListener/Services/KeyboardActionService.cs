using KeyBindingListener.Contracts;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;

namespace KeyBindingListener.Services;

/// <summary>
/// キーボードアクションを実行する
/// </summary>
/// <param name="repository">リポジトリ</param>
/// <param name="actionFactory">アクションファクトリー</param>
public class KeyboardActionService(IKeyboardBindingRepository repository, IActionFactory actionFactory) : IKeyboardActionService
{
    private readonly IKeyboardBindingRepository _keyboardRepository = repository;
    private readonly IActionFactory _actionFactory = actionFactory;

    /// <summary>
    /// 設定されているアクションを実行する
    /// </summary>
    /// <param name="key">仮想キーコード</param>
    /// <returns></returns>
    public async Task ActionAsync(VKeys key)
    {
        var settings = await _keyboardRepository.GetAsync();

        if (!settings.IsEnabledKeyboardBinding || settings.KeyboardActions?.TryGetValue((int)key, out var keyboardAction) is not true)
            return;

        var fileName = string.Empty;
        if (keyboardAction.ActionType is KeyboardActionType.StartProgram)
        {
            fileName = keyboardAction.ProgramPath;
        }
        else if (keyboardAction.ActionType is KeyboardActionType.OpenUrl)
        {
            fileName = keyboardAction.UrlPath;
        }

        if (keyboardAction.ActionType is null || fileName is null)
            return;

        // アクションインスタンスを生成する
        var actor = _actionFactory.Create((KeyboardActionType)keyboardAction.ActionType, fileName);

        // アクション実行
        actor.Action();
    }
}
