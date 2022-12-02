namespace ModLib;

public partial class ModFile
{
    public bool CheckString(string comparison)
    {
        return ReadString(comparison.Length) == comparison;
    }

    public bool CheckString(string comparison, out string read)
    {
        string result = ReadString(comparison.Length);
        read = result;
        return result == comparison;
    }
    
    public bool CheckString(string comparison, string errorMessage)
    {
        bool result = CheckString(comparison, out string read);
        if (!result) Logger.Error(errorMessage, Position, read);
        return result;
    }

    public bool CheckInt(int comparison, bool bigEndian = true)
    {
        return ReadInt(bigEndian) == comparison;
    }

    public bool CheckInt(int comparison, out int read, bool bigEndian = true)
    {
        int result = ReadInt(bigEndian);
        read = result;
        return result == comparison;
    }
    
    public bool CheckInt(int comparison, string errorMessage, bool bigEndian = true)
    {
        bool result = CheckInt(comparison, out int read, bigEndian);
        if (!result) Logger.Error(errorMessage, Position, read);
        return result;
    }
}