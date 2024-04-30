using MagicSettings.Domains;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.Extensions;

/// <summary>
/// キーバインディングアクションの表示の拡張メソッド
/// </summary>
internal static class MagicSettingsExtensions
{
    public static string ToDisplayString(this KeyboardActionType actionType, ResourceLoader resourceLoader)
    {
        return resourceLoader.GetString($"ActionType_{actionType}");
    }
}
