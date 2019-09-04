using System;

namespace ConcurrencyControl.Utils
{
    public class ConsoleUtils
    {
        public static void WriteInf(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void WriteErr(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}
