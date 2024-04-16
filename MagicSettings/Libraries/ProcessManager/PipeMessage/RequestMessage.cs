using System.Diagnostics;
using System.Text.Json;

namespace ProcessManager.PipeMessage;

public class RequestMessage(string command, string arguments = "")
{
    public string Cmd { get; } = command;

    public string Args { get; } = arguments;

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
