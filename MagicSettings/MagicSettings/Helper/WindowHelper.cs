using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace MagicSettings.Helper;

internal static class WindowHelper
{
    // 最小ウィンドウの大きさ
    private static readonly int MinWidth = 800;
    private static readonly int MinHeight = 500;

    public static ElementTheme RootTheme
    {
        get
        {
            foreach (var window in ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                    return rootElement.RequestedTheme;
            }

            return ElementTheme.Default;
        }
        set
        {
            foreach (var window in ActiveWindows)
            {
                if (window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = value;
                }
            }
        }
    }

    public static List<Window> ActiveWindows { get { return _activeWindows; } }

    private static readonly List<Window> _activeWindows = [];

    private static NativeMethods.WinProc? newWndProc = null;
    private static IntPtr oldWndProc = IntPtr.Zero;

    public static void TrackWindow(Window window)
    {
        window.Closed += (sender, args) =>
        {
            _activeWindows.Remove(window);
        };
        _activeWindows.Add(window);
    }

    public static AppWindow GetAppWindow(Window window)
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    public static Window? GetWindowForElement(UIElement element)
    {
        if (element.XamlRoot != null)
        {
            foreach (var window in _activeWindows)
            {
                if (element.XamlRoot == window.Content.XamlRoot)
                {
                    return window;
                }
            }
        }
        return null;
    }

    public static void SetForeground(Window? window)
    {
        if (window is null)
            return;

        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        NativeMethods.SetForegroundWindow(hWnd);
    }

    public static void SetMinWindowSize(Window? window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        newWndProc = new NativeMethods.WinProc(NewWindowProc);
        oldWndProc = NativeMethods.SetWindowLongPtr64(hwnd, NativeMethods.WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
    }

    private static IntPtr NewWindowProc(IntPtr hWnd, NativeMethods.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
    {
        switch (Msg)
        {
            case NativeMethods.WindowMessage.WM_GETMINMAXINFO:
                {
                    var dpi = NativeMethods.GetDpiForWindow(hWnd);
                    float scalingFactor = (float)dpi / 96;

                    NativeMethods.MINMAXINFO minMaxInfo = Marshal.PtrToStructure<NativeMethods.MINMAXINFO>(lParam);
                    minMaxInfo.ptMinTrackSize.x = (int)(MinWidth * scalingFactor);
                    minMaxInfo.ptMinTrackSize.y = (int)(MinHeight * scalingFactor);
                    Marshal.StructureToPtr(minMaxInfo, lParam, true);
                    break;
                }

        }

        return NativeMethods.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
    }

    private class NativeMethods
    {
        [Flags]
        public enum WindowLongIndexFlags : int
        {
            GWL_WNDPROC = -4,
        }

        public enum WindowMessage : int
        {
            WM_GETMINMAXINFO = 0x0024,
        }

        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        public delegate IntPtr WinProc(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc);
        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hwnd);
    }
}