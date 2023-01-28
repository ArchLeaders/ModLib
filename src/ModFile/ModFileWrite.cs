using System.Text;

namespace ModLib;

public partial class ModFile
{
    public void WriteString(string toWrite, int padding = 0)
    {
        byte[] buffer = Encoding.Default.GetBytes(toWrite);
        fileStream.Write(buffer, 0, buffer.Length);
        byte[] pad = new byte[padding];
        fileStream.Write(pad, 0, pad.Length);
    }

    public void WritePascalString(string toWrite, int padding = 0)
    {
        WriteShort((short)(toWrite.Length + padding), true);
        WriteString(toWrite, padding);
    }

    private void WriteBlock(byte[] block, bool bigEndian)
    {
        if (bigEndian) {
            Array.Reverse(block);
        }
        fileStream.Write(block);
    }

    // This does seem redundant, as I could just use a function signature of:
    // WriteObject(object toWrite, bool bigEndian = false)
    // BUT, having an explicit function for each is more legible to me for what I'm writing
    // PLUS, C# will complain if I'm trying to write an INT thinking that it's a LONG because I only ever defined it as an INT
    // And therefore it would only write 4 bytes instead of 8

    public void WriteInt(int toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteUint(uint toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteShort(short toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteUshort(ushort toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteLong(long toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteHalf(Half toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WriteFloat(float toWrite, bool bigEndian = false)
    {
        WriteBlock(BitConverter.GetBytes(toWrite), bigEndian);
    }

    public void WritePadding(int amount, byte byteToWrite = 0)
    {
        for (int i = 0; i < amount; i++) {
            fileStream.WriteByte(byteToWrite);
        }
    }

    public void WriteByte(byte value) => fileStream.WriteByte(value);

    public ModFile GetRemainder()
    {
        long origPosition = Position;
        ModFile remainder = LoadSegment(Position, (int)(Length - Position));
        Seek(origPosition, System.IO.SeekOrigin.Begin);
        return remainder;
    }

    public void AddRemainder(ModFile remainder)
    {
        long origPosition = Position;
        remainder.fileStream.CopyTo(fileStream);
        fileStream.SetLength(Position);
        Seek(origPosition, System.IO.SeekOrigin.Begin);
    }

    public ModFileStatus WriteToFile(string fileLocation)
    {
        using (ModFile file = Create(fileLocation)) {
            if (file.Status != ModFileStatus.Success) return file.Status;

            fileStream.CopyTo(file.fileStream);
            return ModFileStatus.Success;
        }
    }
}
