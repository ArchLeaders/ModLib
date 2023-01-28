namespace ModLib;

public static class ReadLE
{
    private static byte[] ReadBlock(FileStream stream, int size, bool swapEndianness)
    {
        byte[] data = new byte[size];
        stream.Read(data, 0, size);
        if (swapEndianness) {
            Array.Reverse(data);
        }
        return data;
    }

    public static uint Uint(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToUInt32(ReadBlock(stream, 4, swapEndianness));
    }

    public static int Int(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToInt32(ReadBlock(stream, 4, swapEndianness));
    }

    public static long Long(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToInt64(ReadBlock(stream, 8, swapEndianness));
    }

    public static ulong Ulong(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToUInt64(ReadBlock(stream, 8, swapEndianness));
    }

    public static short Short(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToInt16(ReadBlock(stream, 2, swapEndianness));
    }

    public static ushort Ushort(FileStream stream, bool swapEndianness = false)
    {
        return BitConverter.ToUInt16(ReadBlock(stream, 2, swapEndianness));
    }

    public static string NullString(FileStream stream)
    {
        string combined = "";
        while (true) {
            byte currByte = (byte)stream.ReadByte();
            if (currByte == 0) break;

            combined += currByte;
        }

        return combined;
    }

    public static string String(FileStream stream, int length)
    {
        byte[] array = new byte[length];
        stream.Read(array, 0, length);
        return System.Text.Encoding.Default.GetString(array);
    }
}
