using System;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using SFTPService;

namespace SftpDemo
{
    internal class Program
    {
        private static void Main()
        {
            var config = new SftpConfig
            {
                Host = "test.rebex.net",
                Port = 22,
                UserName = "demo",
                Password = "password"
            };
            var sftpService = new SftpService(new NullLogger<SftpService>(), config);

            // list files
            var files = sftpService.ListAllFiles("/pub/example");
            foreach (var file in files)
            {
                if (file.IsDirectory)
                {
                    Console.WriteLine($"Directory: [{file.FullName}]");
                }
                else if (file.IsRegularFile)
                {
                    Console.WriteLine($"File: [{file.FullName}]");
                }
            }

            // download a file
            const string pngFile = @"hi.png";
            File.Delete(pngFile);
            sftpService.DownloadFile(@"/pub/example/imap-console-client.png", pngFile);
            if (File.Exists(pngFile))
            {
                Console.WriteLine($"file {pngFile} downloaded");
            }


            // upload a file // not working for this demo SFTP server due to readonly permission
            var testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt");
            sftpService.UploadFile(testFile, @"/pub/test.txt");
            sftpService.DeleteFile(@"/pub/test.txt");
        }
    }
}
