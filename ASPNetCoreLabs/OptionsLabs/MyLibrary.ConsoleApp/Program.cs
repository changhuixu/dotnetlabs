using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyLibrary.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var myService = serviceProvider.GetRequiredService<IMyService>();
            myService.DoWork();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.AddConsole());
            services.AddMyService(options =>
            {
                options.Option1 = "100 push-ups";
                options.Option2 = true;
                options.Option3 = 3;
            });
        }
    }
}
