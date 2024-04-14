using System.Text.Json.Serialization;

namespace ProcessManager.PipeMessage;

internal class ResponseMessage
{
    [JsonIgnore]
    public bool Result
    {
        get => ResultCode is 0;
    }

    public int ResultCode { get; set; } = -1;

    public string Value { get; set; } = string.Empty;

    public static bool operator true(ResponseMessage x) => x.ResultCode is 0;
    public static bool operator false(ResponseMessage x) => x.ResultCode is not 0;
}
