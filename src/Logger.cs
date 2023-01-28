using System.Text;

namespace ModLib;

public static class Logger
{
    public static string OutputHeader;

    public static string OutputFile {
        get {
            return _outputFile;
        }

        set {
            if (outputModFile != null) {
                outputModFile.Dispose();
            }

            _outputFile = value;
            outputModFile = ModFile.Create(_outputFile);
            if (OutputHeader != null) {
                outputModFile.WriteString("Log file for " + OutputHeader + ":\n");
            }
            else {
                outputModFile.WriteString("Log file:\n");
            }
        }
    }

    private static readonly object ConsoleWriterLock = new object();

    private static string _outputFile;

    private static ModFile outputModFile;

    public static void SendToFile(string value)
    {
        if (outputModFile != null) {
            outputModFile.WriteString(DateTime.Now.ToLongTimeString() + "    ");
            outputModFile.WriteString(value + '\n');
        }
    }

    public static void Log(string value, params object[] args)
    {
        if (args.Length > 0) {
            value = string.Format(value, args);
        }
        lock (ConsoleWriterLock) {
            Console.WriteLine(value);
            SendToFile(value);
        }
    }

    public static void Warn(string value, params object[] args)
    {
        Log(new LogSeg("Warning: ", ConsoleColor.Yellow), new LogSeg(String.Format(value, args)));
    }

    public static void Error(string value, params object[] args)
    {
        Log(new LogSeg("Error: ", ConsoleColor.Red), new LogSeg(String.Format(value, args)));
    }

    private static StringBuilder SegBuilder(params LogSeg[] args)
    {
        StringBuilder str = new StringBuilder();
        foreach (LogSeg arg in args) {
            str.Append(arg.value);
            Console.ForegroundColor = arg.color;
            Console.Write(arg.value);
        }
        Console.ForegroundColor = ConsoleColor.White;
        return str;
    }

    public static void Log(params LogSeg[] args)
    {
        lock (ConsoleWriterLock) {
            var str = SegBuilder(args);
            Console.Write('\n');
            SendToFile(str.ToString());
        }
    }

    public static void ToConsole(string value, params object[] args)
    {
        if (args.Length > 0) {
            value = string.Format(value, args);
        }
        lock (ConsoleWriterLock) {
            Console.WriteLine(value);
        }
    }

    public static void ToConsole(params LogSeg[] args)
    {
        lock (ConsoleWriterLock) {
            SegBuilder(args);
            Console.Write('\n');
        }
    }
}
