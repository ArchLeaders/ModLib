using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLib
{
    public partial class ModFile
    {
        private byte[] ReadBlock(int size, bool bigEndian)
        {
            byte[] data = new byte[size];
            fileStream.Read(data, 0, size);
            if (bigEndian)
            {
                Array.Reverse(data);
            }
            return data;
        }

        public uint ReadUint(bool bigEndian = false)
        {
            return BitConverter.ToUInt32(ReadBlock(4, bigEndian));
        }

        public int ReadInt(bool bigEndian = false)
        {
            return BitConverter.ToInt32(ReadBlock(4, bigEndian));
        }

        public long ReadLong(bool bigEndian = false)
        {
            return BitConverter.ToInt64(ReadBlock(8, bigEndian));
        }

        public ulong ReadUlong(bool bigEndian = false)
        {
            return BitConverter.ToUInt64(ReadBlock(8, bigEndian));
        }

        public short ReadShort(bool bigEndian = false)
        {
            return BitConverter.ToInt16(ReadBlock(2, bigEndian));
        }

        public ushort ReadUshort(bool bigEndian = false)
        {
            return BitConverter.ToUInt16(ReadBlock(2, bigEndian));
        }

        public byte ReadByte()
        {
            return (byte)fileStream.ReadByte();
        }

        public float ReadFloat(bool bigEndian = false)
        {
            return BitConverter.ToSingle(ReadBlock(4, bigEndian));
        }

        public Half ReadHalf(bool bigEndian = false)
        {
            return BitConverter.ToHalf(ReadBlock(2, bigEndian));
        }

        public string ReadNullString()
        {
            string combined = "";
            while (true)
            {
                byte currByte = (byte)fileStream.ReadByte();
                if (currByte == 0) break;

                combined += (char)currByte;
            }

            return combined;
        }

        /// <summary>
        /// Reads a pascal string with a SHORT preceding it
        /// </summary>
        /// <param name="bigEndian"></param>
        /// <param name="security"></param>
        /// <returns></returns>
        /// <exception cref="DataMisalignedException"></exception>
        public string ReadPascalString(bool bigEndian = true, ushort security = 256)
        {
            ushort length = ReadUshort(true);
            if (length > security)
            {
                Logger.Error("Attempting to read string of length {0} at position {1}!", length, Position);
                throw new DataMisalignedException("Potential bad string quashed");
            }

            return ReadString(length);
        }

        /// <summary>
        /// Reads a pascal string with an INT preceding it
        /// </summary>
        /// <param name="bigEndian"></param>
        /// <param name="security"></param>
        /// <returns></returns>
        /// <exception cref="DataMisalignedException"></exception>
        public string ReadBigPascalString(bool bigEndian = true, uint security = 256)
        {
            int length = ReadInt(true);
            if (length > security)
            {
                Logger.Error("Attempting to read string of length {0} at position {1}!", length, Position);
                throw new DataMisalignedException("Potential bad string quashed");
            }

            return ReadString(length);
        }

        public string ReadString(int length)
        {
            byte[] array = new byte[length];
            fileStream.Read(array, 0, length);
            if (array[array.Length - 1] == 0)
            {
                return Encoding.Default.GetString(array, 0, array.Length - 1);
            }
            return Encoding.Default.GetString(array);
        }

        public byte[] ReadArray(int size)
        {
            byte[] array = new byte[size];
            fileStream.Read(array, 0, size);
            return array;
        }

        public void ReadInto(byte[] destination, int size)
        {
            fileStream.Read(destination, 0, size);
        }

        private bool CompareEquality(byte[] sequencedArray, byte[] stitchedArray, int stitchStart)
        {
            int arrLength = sequencedArray.Length;
            for (int i = 0; i < arrLength; i++)
            {
                if (sequencedArray[i] != stitchedArray[(i + stitchStart) % arrLength])
                {
                    return false;
                }
            }
            return true;
        }

        public int Find(byte[] searchQuery)
        {
            long offset = Position;
            //Seek(0, System.IO.SeekOrigin.Begin);
            byte[] buffer = ReadArray(searchQuery.Length); // Read the first sq.Length bytes into the buffer
            int bufferOffset = 0;
            for (int i = searchQuery.Length; i + offset < Length; i++)
            {
                if (CompareEquality(searchQuery, buffer, bufferOffset)) { Seek(offset, System.IO.SeekOrigin.Begin); return i - searchQuery.Length; }
                buffer[bufferOffset] = ReadByte();

                bufferOffset++;
                bufferOffset %= buffer.Length;
            }

            Seek(offset, System.IO.SeekOrigin.Begin);
            return -1;
        }

        public int Find(string searchQuery)
        {
            return Find(Encoding.Default.GetBytes(searchQuery));
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
    }
}
