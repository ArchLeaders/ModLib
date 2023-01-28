#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ModLib;

public enum ModFileStatus
{
    Success,
    UnauthorizedAccess,
    IO,
    Other
}

public partial class ModFile : IDisposable
{
    public Stream fileStream { get; internal set; }

    public long Position => fileStream.Position;
    public long Length => fileStream.Length;

    public string Location => ((FileStream)fileStream).Name;

    public ModFileStatus Status;

    public ModFile() { }

    public ModFile(byte[] data)
    {
        fileStream = new MemoryStream(data);
    }

    public ModFile(Stream stream)
    {
        fileStream = stream;
    }

    public void Seek(long offset, SeekOrigin origin)
    {
        fileStream.Seek(offset, origin);
    }

    public void Dispose()
    {
        fileStream?.Dispose();
    }
}
