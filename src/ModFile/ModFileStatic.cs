﻿namespace ModLib;

public partial class ModFile
{
    /// <summary>
    /// Sets the file size threshold for which files will not be backed into memory.
    /// </summary>
    public static int Threshold = 1000000000;

    public static ModFile Open(string fileLocation, bool loadEverything = false)
    {
        ModFile file = new ModFile();

        try {
            if (loadEverything) {
                FileInfo diskFile = new FileInfo(fileLocation);
                bool shouldStream = false;
                if (diskFile.Length > Threshold) // Greater than 1.0GB
                {
                    if ((Invocations.GetAvailableMemory() <= (ulong)diskFile.Length * 3) || diskFile.Length > int.MaxValue) // Check that we have 3x the necessary space and that the file is less than the max size of a byte array
                    {
                        shouldStream = true;
                    }
                }

                if (!shouldStream) {
                    using (FileStream fileOnDisk = File.Open(fileLocation, FileMode.Open)) {
                        if (diskFile.Length > 1000000000) {
                            float niceSize = (float)diskFile.Length / 1000000000;
                            string niceNumber = String.Format("{0:.##}", niceSize);
                            Console.WriteLine("Loading " + diskFile.Name + " (" + niceNumber + " GB) into memory, this may take a few seconds.");
                        }
                        file.fileStream = new MemoryStream();
                        file.fileStream.SetLength(fileOnDisk.Length);
                        fileOnDisk.Read(((MemoryStream)file.fileStream).GetBuffer(), 0, (int)fileOnDisk.Length);
                    }

                    return file;
                }
            }

            file.fileStream = File.Open(fileLocation, FileMode.Open);
        }
        catch (UnauthorizedAccessException ex) {
            file.Status = ModFileStatus.UnauthorizedAccess;
        }
        catch (IOException) {
            file.Status = ModFileStatus.IO;
        }
        catch (Exception ex) {
            file.Status = ModFileStatus.Other;
            Logger.Log(new LogSeg("An error has occurred:\n{0}", ConsoleColor.Red, ex.Message));
        }

        return file;
    }

    public static ModFile Create()
    {
        ModFile file = new ModFile();
        file.fileStream = new MemoryStream();
        return file;
    }

    /// <summary>
    /// Creates a ModFile instance that uses a stream in memory, with the specified capacity.
    /// </summary>
    /// <param name="capacity"></param>
    /// <returns></returns>
    public static ModFile Create(int capacity)
    {
        ModFile file = new ModFile();
        file.fileStream = new MemoryStream(capacity);
        return file;
    }

    /// <summary>
    /// Creates a ModFile instance, but writes all buffer content direct to disk. Useful for large files.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static ModFile Create(string location)
    {
        ModFile file = new ModFile();
        try {
            file.fileStream = File.Create(location);
            file.Status = ModFileStatus.Success;
        }
        catch (UnauthorizedAccessException) {
            file.Status = ModFileStatus.UnauthorizedAccess;
        }
        catch (IOException) {
            file.Status = ModFileStatus.IO;
        }
        catch (Exception ex) {
            file.Status = ModFileStatus.Other;
            Logger.Log(new LogSeg("An error has occurred:\n{0}", ConsoleColor.Red, ex.Message));
        }

        return file;
    }
}
