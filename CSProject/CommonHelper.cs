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
            Console.WriteLine(GetDateTimeInMillisecond() + "\t" + msg);
        }

        public static void OutputWarningMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            OutputMessage(msg);
            Console.ResetColor();
        }

        public static void OutputErrorMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            OutputMessage(msg);
            Console.ResetColor();
        }

        public static void OutputSuccessMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            OutputMessage(msg);
            Console.ResetColor();
        }

        public static string GetDateTimeInMillisecond()
        {
            DateTime currentTime = DateTime.Now;
            return GetDateTimeInMillisecond(currentTime);
        }

        public static string GetDateTimeInMillisecond(DateTime currentTime)
        {
            return currentTime.ToString("yyyy-MM-dd hh:mm:ss.fff tt");
        }
    }
}
