using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MagicSettings.Repositories.Helper;

internal class JsonOptionHelper
{
    public static JsonSerializerOptions GetOption()
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
        };
        return options;
    }
}
