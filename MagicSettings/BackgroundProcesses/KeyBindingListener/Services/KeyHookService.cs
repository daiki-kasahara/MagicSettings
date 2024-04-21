using System.Diagnostics;
using KeyBindingListener.Events;

namespace KeyBindingListener.Services;

internal class KeyHookService
{
    private static bool IsWinKey = false;
    private static bool IsAltKey = false;

    public void OnKeyDown(object? _, KeyboardHookEventArgs ea)
    {
        switch (ea.Key)
        {
            case Keys.LWin or Keys.RWin:
                {
                    IsWinKey = true;
                    break;
                }
            case Keys.LMenu:
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

    public void OnKeyUp(object? _, KeyboardHookEventArgs ea)
    {
        switch (ea.Key)
        {
            case Keys.LWin or Keys.RWin:
                {
                    IsWinKey = false;
                    break;
                }
            case Keys.Alt:
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
