using System.Diagnostics;
using KeyBindingListener.Events;

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
            case Keys.LWin or Keys.RWin:
                {
                    IsWinKey = true;
                    break;
                }
            case Keys.LMenu or Keys.RMenu:
                {
                    IsAltKey = true;
                    break;
                }
            case Keys.D:
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
            case Keys.LWin or Keys.RWin:
                {
                    IsWinKey = false;
                    break;
                }
            case Keys.LMenu or Keys.RMenu:
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
