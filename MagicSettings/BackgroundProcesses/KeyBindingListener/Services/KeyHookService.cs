using KeyBindingListener.Contracts;
using KeyBindingListener.Events;
using MagicSettings.Domains;

namespace KeyBindingListener.Services;

/// <summary>
/// キーボードイベントのメイン処理
/// </summary>
public class KeyHookService(IKeyboardActionService keyboardActionService)
{
    private static bool IsWinKey = false;
    private static bool IsAltKey = false;

    private readonly IKeyboardActionService _keyboardActionService = keyboardActionService;

    /// <summary>
    /// キーダウンイベント発火時の処理
    /// </summary>
    /// <param name="_"></param>
    /// <param name="ea">キー情報</param>
    public void OnKeyDown(object? _, KeyboardHookEventArgs ea)
    {
        switch (ea.Key)
        {
            case VKeys.LeftWindows or VKeys.RightWindows:
                {
                    // フラグを立てる
                    IsWinKey = true;
                    break;
                }
            case VKeys.LeftMenu or VKeys.RightMenu:
                {
                    // フラグを立てる
                    IsAltKey = true;
                    break;
                }
            default:
                {
                    // アクション実行条件を満たしているかチェック
                    if (!IsWinKey || !IsAltKey || !Enum.IsDefined(ea.Key))
                        break;

                    // キーの応答を早くするため、非同期で実行し、結果は Wait しない
                    Task.Run(() => _keyboardActionService.ActionAsync(ea.Key));

                    break;
                }
        }

        // 他のプログラムにキーを転送する場合は0
        ea.RetCode = 0;
    }

    /// <summary>
    /// キーボードアップイベント発火時の処理
    /// </summary>
    /// <param name="_"></param>
    /// <param name="ea">キー情報</param>
    public void OnKeyUp(object? _, KeyboardHookEventArgs ea)
    {
        switch (ea.Key)
        {
            case VKeys.LeftWindows or VKeys.RightWindows:
                {
                    // フラグを降ろす
                    IsWinKey = false;
                    break;
                }
            case VKeys.LeftMenu or VKeys.RightMenu:
                {
                    // フラグを降ろす
                    IsAltKey = false;
                    break;
                }
            default:
                {
                    break;
                }
        }

        // 他のプログラムにキーを転送する場合は0
        ea.RetCode = 0;
    }
}
