using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThrottledWebApi.ClientDemo
{
    public interface IThrottledHttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers">A list of integers</param>
        /// <param name="requestLimit">Max number of concurrent requests</param>
        /// <param name="limitingPeriodInSeconds">per second or per n seconds</param>
        /// <returns></returns>
        Task<PrimeNumberResult[]> GetPrimeNumberResults(List<long> numbers, int requestLimit = 2, int limitingPeriodInSeconds = 1);
    }

    public class ThrottledHttpClient : IThrottledHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = @"http://localhost:5000/api";

        public ThrottledHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers">A list of integers</param>
        /// <param name="requestLimit">Max number of concurrent requests</param>
        /// <param name="limitingPeriodInSeconds">per second or per n seconds</param>
        /// <returns></returns>
        public async Task<PrimeNumberResult[]> GetPrimeNumberResults(List<long> numbers, int requestLimit = 2, int limitingPeriodInSeconds = 1)
        {
            var throttler = new SemaphoreSlim(requestLimit);
            var tasks = numbers.Select(async n =>
            {
                await throttler.WaitAsync();

                var task = _httpClient.GetStringAsync($"{_baseUrl}/values/isPrime?number={n}");
                _ = task.ContinueWith(async s =>
                {
                    await Task.Delay(1000 * limitingPeriodInSeconds);
                    Console.WriteLine($"\t\t {n} waiting");
                    throttler.Release();
                });
                try
                {
                    var isPrime = await task;
                    Console.WriteLine($"{n}");
                    return new PrimeNumberResult(n, isPrime);
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine($"\t\t\t {n} error out");
                    return new PrimeNumberResult(n, "NA");
                }
            });
            return await Task.WhenAll(tasks);
        }
    }

    public class PrimeNumberResult
    {
        public long Number { get; set; }
        public string IsPrime { get; set; }

        public PrimeNumberResult()
        {
        }

        public PrimeNumberResult(long n, string isPrime)
        {
            Number = n;
            IsPrime = isPrime;
        }
    }
}
