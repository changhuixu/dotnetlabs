using System;
using System.IO;

namespace HelloWorld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileName = "a.txt";
            if (args.Length > 0)
            {
                fileName = $"{args[0]}.txt";
            }
            Console.WriteLine("Hello World -- Start");
            const string dir = @"App_Data";
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, fileName);
            File.WriteAllText(filePath, "Hi");
            Console.WriteLine($"file path: {filePath}");
            Console.WriteLine("Hello World -- End");

            //PrintDirectories();
        }

        private static void PrintDirectories()
        {
            var rootDir = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            Console.WriteLine(rootDir);
            // file:///C:/Projects/github/dotnetlabs/CSharpLabs/RunExeFromWebApi/HelloWorld/bin/Debug/HelloWorld.exe
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(baseDir);
            // C:\Projects\github\dotnetlabs\CSharpLabs\RunExeFromWebApi\HelloWorld\bin\Debug\
        }
    }
}
