using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ClientPipe
{
    public async Task<bool> SendTerminateMessage(MyProcesses process)
    {
        try
        {
            using var pipeClient = new NamedPipeClientStream(".", $"MagicSettings-{process}", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            await pipeClient.ConnectAsync();
            var ss = new StreamString(pipeClient);

            while (true)
            {
                // 入力された文字列を送信する
                var request = new RequestMessage("terminate");
                var write = ss.WriteString(request.Serialize());
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
        }
        catch (OverflowException ofex)
        {
            return false;
        }
        catch (IOException ioe)
        {
            // 送信失敗
            return false;
        }
    }

    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
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
