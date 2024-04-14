using System;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace MagicSettings.Helper;

internal class WindowHelper
{
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
}
