using System.Text.Json.Serialization;

namespace ProcessManager.PipeMessage;

internal class ResponseMessage
{
    [JsonIgnore]
    public bool Result
    {
        get => ReturnCode is 0;
    }

    public int ReturnCode { get; set; } = -1;

    public string ReturnParameters { get; set; } = string.Empty;

    public static bool operator true(ResponseMessage x) => x.ReturnCode is 0;
    public static bool operator false(ResponseMessage x) => x.ReturnCode is not 0;
}
