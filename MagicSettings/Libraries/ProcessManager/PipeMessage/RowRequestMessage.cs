namespace ProcessManager.PipeMessage;

/// <summary>
/// リクエストメッセージの生データ
/// </summary>
internal class RowRequestMessage
{
    public string Cmd { get; set; } = string.Empty;
    public string Args { get; set; } = string.Empty;
}
