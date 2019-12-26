using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClientSideApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                throw new ArgumentException("Please provide file name to post");
            }
            var services = new ServiceCollection().AddLogging(c => c.AddConsole());
            services.AddSingleton(new DemoHttpClientSettings
            {
                Url = args[0]
            });
            services.AddHttpClient<IDemoHttpClient, DemoHttpClient>();
            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IDemoHttpClient>();
            var guid = await client.UploadFile(args[1]);
            await client.DownloadFile(guid);
            Console.WriteLine();
        }
    }
}
