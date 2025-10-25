namespace TestProject.P3DFile.Helpers;

public static class SmallWorkBuffer
{
    public const int BufferSize = 8;

    private static readonly ThreadLocal<byte[]> _SmallWorkBuffer = new ThreadLocal<byte[]>(() => new byte[8]);
    public static byte[] Get(int count)
    {
        if (count < 0 || count > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        byte[]? bufferValue = _SmallWorkBuffer.Value;
        if (bufferValue == null)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferValue));

        }
        return bufferValue;
    }
    public static byte[] ReadBytes(Stream stream, int count)
    {
        if (count < 0 || count > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        byte[]? bufferValue = _SmallWorkBuffer.Value;


        if (bufferValue == null) { throw new ArgumentOutOfRangeException(nameof(bufferValue)); }



        if (stream.Read(bufferValue, 0, count) != count)
        {
            throw new EndOfStreamException();
        }
        return bufferValue;
    }
}
