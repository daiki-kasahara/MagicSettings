using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using ProcessManager.PipeMessage;

namespace ProcessManager;

internal class ClientPipe
{
    public async Task<bool> CheckExistedMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Check"), 10000);

    public async Task<bool> SendTerminateMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Terminate"), 5000);

    public async Task<bool> SendRequestMessageAsync(MyProcesses process, RequestMessage requestMessage) =>
        await SendMessageToServerAsync(process, requestMessage, 5000);

    private async Task<bool> SendMessageToServerAsync(MyProcesses process, RequestMessage message, int timeout)
    {
        try
        {
            using var pipeClient = new NamedPipeClientStream(".", $"MagicSettings-{process}", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            await pipeClient.ConnectAsync(timeout);
            var ss = new StreamString(pipeClient);

            // 入力された文字列を送信する
            var write = ss.WriteString(message.Serialize());

            // 応答待ち
            var read = ss.ReadString();

            try
            {
                return JsonSerializer.Deserialize<ResponseMessage>(read)?.Result ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    private class StreamString
    {
        private readonly Stream ioStream;
        private readonly UTF8Encoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UTF8Encoding();
        }

        public string ReadString()
        {
            var len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            var inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            var outBuffer = streamEncoding.GetBytes(outString);
            var len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
