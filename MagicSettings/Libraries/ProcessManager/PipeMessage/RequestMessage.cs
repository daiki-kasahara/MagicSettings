using System.Diagnostics;
using System.Text.Json;

namespace ProcessManager.PipeMessage;

internal class RequestMessage(string command, string arguments = "")
{
    public string Cmd { get; } = command;

    public string Args { get; } = arguments;

    //private readonly JsonSerializerOptions _options = new()
    //{
    //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    //};

    public string Serialize() => JsonSerializer.Serialize(this);

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
