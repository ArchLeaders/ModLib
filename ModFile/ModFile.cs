using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ModLib
{
    public partial class ModFile : IDisposable
    {
        public Stream fileStream { get; internal set; }

        public void Seek(long offset, SeekOrigin origin) => fileStream.Seek(offset, origin);

        public long Position => fileStream.Position;

        public long Length => fileStream.Length;

        public string Location => ((FileStream)fileStream).Name;

        public ModFileStatus Status;

        public ModFile(byte[] data) 
        {
            fileStream = new MemoryStream(data);
        }

        public ModFile() { }

        public void Dispose()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
            }
        }
    }

    public enum ModFileStatus
    {
        Success,
        UnauthorizedAccess,
        IO,
        Other
    }
}
