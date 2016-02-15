using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProject
{
    class CommonHelper
    {
        public static void OutputMessage(string msg)
        {
            Console.WriteLine(GetDateTimeInMillisecond() + " " + msg);
        }

        public static void OutputWarningMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(GetDateTimeInMillisecond() + " " + msg);
            Console.ResetColor();
        }

        public static void OutputErrorMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(GetDateTimeInMillisecond() + " " + msg);
            Console.ResetColor();
        }

        public static void OutputSuccessMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(GetDateTimeInMillisecond() + " " + msg);
            Console.ResetColor();
        }

        public static string GetDateTimeInMillisecond()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.ToString("yyyy-MM-dd hh:mm:ss.fff tt");
        }
    }
}
