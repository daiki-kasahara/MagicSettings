using System.Runtime.InteropServices;

namespace KeyBindingListener.Helpers;

internal static class NativeMethods
{
    public static readonly int WH_KEYBOARD_LL = 0x0D;
    public static readonly int WM_KEYBOARD_DOWN = 0x100;
    public static readonly int WM_KEYBOARD_UP = 0x101;
    public static readonly int WM_SYSKEY_DOWN = 0x104;
    public static readonly int WM_SYSKEY_UP = 0x105;

    public delegate IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookCallback lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

}
