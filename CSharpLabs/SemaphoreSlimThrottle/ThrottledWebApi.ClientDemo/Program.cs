using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ThrottledWebApi.ClientDemo
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection().AddHttpClient();
            services.AddHttpClient<IThrottledHttpClient, ThrottledHttpClient>();
            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetService<IThrottledHttpClient>();
            var numbers = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 11, 13, 17, 19, 23, 29, 31, 41, 43, 1763 };
            var results = await client.GetPrimeNumberResults(numbers);
            foreach (var result in results)
            {
                Console.WriteLine($"{result.Number} is a prime number? \t {result.IsPrime}.");
            }
        }
    }
}
