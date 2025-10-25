using System.Text;
using TestProject.P3DFile.Models;

namespace TestProject.P3DFile.Helpers;

public static class StreamHelper
{
    public static Encoding DefaultEncoding => Encoding.ASCII;

    public static byte ReadValueU8(this Stream stream)
    {
        return SmallWorkBuffer.ReadBytes(stream, 1)[0];
    }

    public static uint ReadValueU32(this Stream stream)
    {
        uint num = BitConverter.ToUInt32(SmallWorkBuffer.ReadBytes(stream, 4), 0);
        return num;
    }
    public static string ReadString(this Stream stream, int size, Encoding? encoding = null)
    {
        encoding ??= DefaultEncoding;
        return stream.ReadStringInternalStatic(encoding, size, trailingNull: false);
    }

    public static string ReadString(this Stream stream, int size, bool trailingNull, Encoding encoding)
    {
        return stream.ReadStringInternalStatic(encoding, size, trailingNull);
    }

    internal static string ReadStringInternalStatic(this Stream stream, Encoding encoding, int size, bool trailingNull)
    {
        byte[] array = stream.ReadBytes(size);
        string text = encoding.GetString(array, 0, array.Length);
        if (trailingNull)
        {
            int num = text.IndexOf('\0');
            if (num >= 0)
            {
                text = text[..num];
            }
        }
        return text;
    }

    public static byte[] ReadBytes(this Stream stream, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException("length");
        }
        byte[] array = new byte[length];
        if (stream.Read(array, 0, length) != length)
        {
            throw new EndOfStreamException();
        }
        return array;
    }
    public static string ReadStringAlignedU8(this Stream stream)
    {
        byte b = stream.ReadValueU8();
        if (b == 0)
        {
            return "";
        }
        return stream.ReadString(b, trailingNull: false, Encoding.ASCII);
    }

}
