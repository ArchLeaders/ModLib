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

        public ModFileStatus Status;

        public void Dispose()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
            }
        }

        public ModFile LoadSegment(long offset, int size)
        {
            ModFile segment = new ModFile();
            segment.fileStream = new MemoryStream(size);

            fileStream.Seek(offset, SeekOrigin.Begin);
            
            byte[] buffer = new byte[size];
            fileStream.Read(buffer, 0, buffer.Length);
            segment.fileStream.Write(buffer, 0, buffer.Length);


            segment.fileStream.Position = 0;

            return segment;
        }

        public void FlushToFile(string filename)
        {
            if (fileStream is MemoryStream)
            {
                using (FileStream stream = File.Create(filename))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    stream.Seek(0, SeekOrigin.Begin);
                    
                    fileStream.CopyTo(stream);
                }
            }
            else
            {
                Console.WriteLine("Unnecessary FlushToFile call that will not be executed");
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
