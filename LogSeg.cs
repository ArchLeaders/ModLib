using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLib
{
    public class LogSeg
    {
        public string value { get; private set; }
        public ConsoleColor color { get; private set; }

        public LogSeg(string value, params string[] args)
        {
            if (args.Length != 0)
            {
                value = string.Format(value, args);
            }
            this.value = value;
            this.color = ConsoleColor.White;
        }

        public LogSeg(string value, ConsoleColor color, params string[] args)
        {
            if (args.Length != 0)
            {
                value = string.Format(value, args);
            }
            this.value = value;
            this.color = color;
        }
    }
}
