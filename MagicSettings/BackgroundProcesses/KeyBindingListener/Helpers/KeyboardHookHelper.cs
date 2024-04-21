using System.Diagnostics;
using System.Runtime.InteropServices;
using KeyBindingListener.Events;

namespace KeyBindingListener.Helpers;

internal class KeyboardHookHelper
{
    // キーボードイベントハンドラ
    public event EventHandler<KeyboardHookEventArgs> OnKeyDown = delegate { };
    public event EventHandler<KeyboardHookEventArgs> OnKeyUp = delegate { };

    // プロシージャのハンドル
    private IntPtr _hookHandle = IntPtr.Zero;

    // フック時のコールバック関数
    private NativeMethods.HookCallback? _callback = null;

    public void Hook()
    {
        _callback = CallbackProc;
        using var process = Process.GetCurrentProcess();
        using var module = process.MainModule;

        if (module is null)
            return;

        _hookHandle = NativeMethods.SetWindowsHookEx(
           NativeMethods.WH_KEYBOARD_LL,
           _callback,
           NativeMethods.GetModuleHandle(module.ModuleName),
           0
       );
    }

    public void UnHook()
    {
        NativeMethods.UnhookWindowsHookEx(_hookHandle);
        _hookHandle = IntPtr.Zero;
    }

    private IntPtr CallbackProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        var args = new KeyboardHookEventArgs();
        var key = (Keys)(short)Marshal.ReadInt32(lParam);
        args.Key = key;

        int wp;
        checked
        {
            wp = (int)wParam;
        }

        switch (wp)
        {
            case NativeMethods.WM_KEYBOARD_DOWN:
            case NativeMethods.WM_SYSKEY_DOWN:
                {
                    OnKeyDown(this, args);
                    break;
                }
            case NativeMethods.WM_KEYBOARD_UP:
            case NativeMethods.WM_SYSKEY_UP:
                {
                    OnKeyUp(this, args);
                    break;
                }
            default:
                break;
        }

        return args.RetCode is 0 ? NativeMethods.CallNextHookEx(_hookHandle, nCode, wParam, lParam) : 1;
    }
}
