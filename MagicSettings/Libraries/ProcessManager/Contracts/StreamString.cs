using System.Text;

namespace ProcessManager.Contracts;

/// <summary>
/// パイプ通信でやり取りするメッセージを加工するクラス
/// </summary>
/// <param name="ioStream"></param>
internal class StreamString(Stream ioStream)
{
    private readonly Stream ioStream = ioStream;
    private readonly UTF8Encoding streamEncoding = new();

    /// <summary>
    /// ストリームから読み込む
    /// </summary>
    /// <returns></returns>
    public string ReadString()
    {
        var len = ioStream.ReadByte() * 256;
        len += ioStream.ReadByte();
        var inBuffer = new byte[len];
        ioStream.Read(inBuffer, 0, len);

        return streamEncoding.GetString(inBuffer);
    }

    /// <summary>
    /// ストリームに書き込む
    /// </summary>
    /// <param name="outString"></param>
    /// <returns></returns>
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
