using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThrottledWebApi.IntegrationTests.TestBase;

namespace ThrottledWebApi.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests
    {
        private readonly HttpClient _httpClient = new TestServerFixture().Client;

        [DataTestMethod]
        [DataRow(1, "false")]
        [DataRow(2, "true")]
        [DataRow(11, "true")]
        [DataRow(12, "false")]
        public async Task TestWithDataSource(int n, string isPrime)
        {
            var response = await _httpClient.GetAsync($"api/values/isPrime?number={n}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(isPrime, result);
        }

        [TestMethod]
        public async Task ExpectExceptionWhenExceedRateLimit()
        {
            var numbers = new List<long> { 1, 12, 11 };
            var allTasks = numbers.Select(n => Task.Run(async () =>
            {
                var result = await _httpClient.GetStringAsync($"api/values/isPrime?number={n}");
                Console.WriteLine($"{n} is a prime number? {result}");
            })).ToList();
            async Task ConcurrentApiRequests() => await Task.WhenAll(allTasks);
            var e = await Assert.ThrowsExceptionAsync<HttpRequestException>(ConcurrentApiRequests);
            Assert.AreEqual("Response status code does not indicate success: 429 (Too Many Requests).", e.Message);
        }

        [TestMethod]
        public async Task ExpectHttpHeadersWhenExceedRateLimit()
        {
            var numbers = new List<long> { 1, 12, 11 };
            var allTasks = numbers.Select(n => Task.Run(async () =>
            {
                var response = await _httpClient.GetAsync($"api/values/isPrime?number={n}");
                return new
                {
                    Number = n,
                    Headers = response.Headers.ToList()
                };
            })).ToList();

            var results = await Task.WhenAll(allTasks);

            // assert
            var retryHeaders = results.SelectMany(x => x.Headers).Where(x => x.Key == "Retry-After").ToList();
            Assert.AreEqual(1, retryHeaders.Count);
            Assert.AreEqual("1", string.Join(", ", retryHeaders[0].Value));
            var xRateLimitHeaders = results.SelectMany(x => x.Headers).Where(x => x.Key.StartsWith("X-Rate-Limit")).ToList();
            Assert.AreEqual(6, xRateLimitHeaders.Count);

            // auxiliary method to print out all headers.
            foreach (var result in results)
            {
                Console.WriteLine($"\r\nHTTP Response Headers for number = {result.Number}:");
                foreach (var (key, value) in result.Headers)
                {
                    Console.WriteLine($"\t{key}: {string.Join(", ", value)}");
                }
            }
        }
/* Test Output

TEST Server Started.

HTTP Response Headers for number = 1:
	Retry-After: 1

HTTP Response Headers for number = 12:
	X-Rate-Limit-Limit: 1s
	X-Rate-Limit-Remaining: 1
	X-Rate-Limit-Reset: 2019-09-17T18:51:02.4401731Z

HTTP Response Headers for number = 11:
	X-Rate-Limit-Limit: 1s
	X-Rate-Limit-Remaining: 0
	X-Rate-Limit-Reset: 2019-09-17T18:51:02.4401731Z

*/

        [TestMethod]
        public async Task TestWithSemaphoreSlim()
        {
            var numbers = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 11, 13, 17, 19, 23, 29, 31, 41, 43, 1763, 7400854980481283 };

            var throttler = new SemaphoreSlim(2);
            var tasks = numbers.Select(async n =>
            {
                await throttler.WaitAsync();

                var task = _httpClient.GetStringAsync($"api/values/isPrime?number={n}");
                _ = task.ContinueWith(async s =>
                {
                    await Task.Delay(1000);
                    throttler.Release();
                });
                try
                {
                    var isPrime = await task;
                    return new
                    {
                        Number = n,
                        IsPrime = isPrime
                    };
                }
                catch (HttpRequestException)
                {
                    return new
                    {
                        Number = n,
                        IsPrime = "NA"
                    };
                }
            });
            var results = await Task.WhenAll(tasks);

            // assert
            var expectedPrimeNumbers = new List<long> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 41, 43 };
            var expectedNonPrimeNumbers = new List<long> { 1, 4, 6, 8, 9, 10, 1763, 7400854980481283 };
            CollectionAssert.AreEquivalent(expectedPrimeNumbers, results.Where(x => x.IsPrime == "true").Select(x => x.Number).ToList());
            CollectionAssert.AreEquivalent(expectedNonPrimeNumbers, results.Where(x => x.IsPrime == "false").Select(x => x.Number).ToList());
            Assert.AreEqual("NA", results.First(x => x.Number == 0).IsPrime);
        }
    }
}
