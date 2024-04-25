using System.Text.Json;

namespace ProcessManager.PipeMessage;

public class RequestMessage(string command, string arguments = "")
{
    public string Cmd { get; } = command;

    public string Args { get; } = arguments;

    public string Serialize() => JsonSerializer.Serialize(this);
}
