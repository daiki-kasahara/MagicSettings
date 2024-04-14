using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ProcessManager.PipeMessage;

internal class RequestMessage(string command, string arguments = "")
{
    public string Command { get; } = command;

    public string Arguments { get; } = arguments;

    private readonly JsonSerializerOptions _options = new()
    {
        AllowTrailingCommas = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
    };

    public string Serialize() => JsonSerializer.Serialize(this, _options);

    public static RequestMessage? Deserialize(string? serialized)
    {
        try
        {
            if (string.IsNullOrEmpty(serialized))
            {
                return null;
            }
            return JsonSerializer.Deserialize<RequestMessage>(serialized);
        }
        catch (JsonException e)
        {
            Debug.WriteLine(e);
        }
        return null;
    }
}
