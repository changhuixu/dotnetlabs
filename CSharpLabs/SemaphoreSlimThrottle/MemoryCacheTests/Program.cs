using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheTests
{
    internal class Program
    {
        private static void Main()
        {
            // demo of race condition of GetOrCreate when cache is null.
            var cache = new MemoryCache(new MemoryCacheOptions());
            var rand = new Random();
            var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
            {
                var ret = cache.GetOrCreate("key", entry => rand.Next());
                Console.WriteLine($"Task {i,2}: {ret}");
            })).ToArray();
            Task.WaitAll(tasks);    
        }
    }
}
