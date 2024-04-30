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

        // 設定が無効もしくは、key に紐づく設定がされていない場合は何もしない
        if (!settings.IsEnabledKeyboardBinding || settings.KeyboardActions?.TryGetValue((int)key, out var keyboardAction) is not true)
            return;

        var fileName = string.Empty;
        if (keyboardAction.ActionType is KeyboardActionType.StartProgram)
        {
            fileName = keyboardAction.ProgramPath;
        }
        else if (keyboardAction.ActionType is KeyboardActionType.OpenUrl)
        {
            fileName = keyboardAction.Url;
        }

        // アクション情報が null もしくは、Disableの場合、何もしない
        if (keyboardAction.ActionType is null || fileName is null || !keyboardAction.IsEnabled)
            return;

        // アクションインスタンスを生成する
        var actor = _actionFactory.Create((KeyboardActionType)keyboardAction.ActionType, fileName);

        // アクション実行
        actor.Action();
    }
}
