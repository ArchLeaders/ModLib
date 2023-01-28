namespace ModLib;

public static class WriteLE
{
    private static void WriteBlock(FileStream stream, byte[] toWrite, bool swapEndianness)
    {
        if (swapEndianness) {
            Array.Reverse(toWrite);
        }

        stream.Write(toWrite);
    }

    public static void Short(FileStream stream, short toWrite, bool swapEndianness = false)
    {
        WriteBlock(stream, BitConverter.GetBytes(toWrite), swapEndianness);
    }

    public static void Int(FileStream stream, int toWrite, bool swapEndianness = false)
    {
        WriteBlock(stream, BitConverter.GetBytes(toWrite), swapEndianness);
    }

    public static void Long(FileStream stream, long toWrite, bool swapEndianness = false)
    {
        WriteBlock(stream, BitConverter.GetBytes(toWrite), swapEndianness);
    }
}
