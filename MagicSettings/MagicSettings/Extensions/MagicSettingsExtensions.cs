using MagicSettings.Domains;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.Extensions;

internal static class MagicSettingsExtensions
{
    public static string ToDisplayString(this KeyboardActionType actionType, ResourceLoader resourceLoader)
    {
        return resourceLoader.GetString($"ActionType_{actionType}");
    }
}
