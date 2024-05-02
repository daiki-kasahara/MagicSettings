using System.Text.Json;

namespace ProcessManager.PipeMessage;

/// <summary>
/// リクエストメッセージ
/// </summary>
/// <param name="command"></param>
/// <param name="arguments"></param>
public class RequestMessage(string command, string arguments = "")
{
    public string Cmd { get; } = command;

    public string Args { get; } = arguments;

    public string Serialize() => JsonSerializer.Serialize(this);
}
