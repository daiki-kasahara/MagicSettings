using System.Collections.Generic;

namespace MagicSettings.Models;

internal static class CustomDisplayKeys
{
    public static readonly IReadOnlyDictionary<int, string> Keys = new Dictionary<int, string>()
    {
        { 0x08, "Backspace" },
        { 0x0D, "Enter" },
        { 0x10, "Shift" },
        { 0x11, "Ctrl" },
        { 0x14, "CapsLock" },
        { 0x21, "PageUp" },
        { 0x22, "PageDown" },
        { 0x30, "0" },
        { 0x31, "1" },
        { 0x32, "2" },
        { 0x33, "3" },
        { 0x34, "4" },
        { 0x35, "5" },
        { 0x36, "6" },
        { 0x37, "7" },
        { 0x38, "8" },
        { 0x39, "9" },
        { 0xBA, ";" },
        { 0xBB, "=" },
        { 0xBC, "," },
        { 0xBD, "-" },
        { 0xBE, "." },
        { 0xBF, "/" },
        { 0xC0, "`" },
        { 0xDB, "[" },
        { 0xDC, "\\" },
        { 0xDD, "]" },
        { 0xDE, "'" },
    };
}
