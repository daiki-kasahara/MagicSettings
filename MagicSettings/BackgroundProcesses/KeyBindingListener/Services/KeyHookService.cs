using System.Diagnostics;
using KeyBindingListener.Events;
using MagicSettings.Domains;

namespace KeyBindingListener.Services;

/// <summary>
/// キーボードイベントのメイン処理
/// </summary>
internal class KeyHookService
{
    private static bool IsWinKey = false;
    private static bool IsAltKey = false;

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
                    IsWinKey = true;
                    break;
                }
            case VKeys.LeftMenu or VKeys.RightMenu:
                {
                    IsAltKey = true;
                    break;
                }
            case VKeys.D:
                {
                    if (IsWinKey && IsAltKey)
                        Process.Start(new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            FileName = "ms-settings:privacy-webcam",
                        });

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
                    IsWinKey = false;
                    break;
                }
            case VKeys.LeftMenu or VKeys.RightMenu:
                {
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
