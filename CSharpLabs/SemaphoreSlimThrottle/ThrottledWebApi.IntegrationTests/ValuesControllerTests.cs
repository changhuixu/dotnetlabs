using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThrottledWebApi.IntegrationTests.TestBase;

namespace ThrottledWebApi.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests
    {
        private readonly TestServerFixture _fixture = new TestServerFixture();

        [DataTestMethod]
        [DataRow(1, "false")]
        [DataRow(12, "false")]
        [DataRow(11, "true")]
        public async Task TestWithDataSource(int n, string isPrime)
        {
            var response = await _fixture.Client.GetAsync($"api/values/isPrime?number={n}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(isPrime, result);
        }

        [TestMethod]
        public async Task TestWithTasks()
        {
            var numbers = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 17, 19, 23, 29, 31, 41, 43, 1763, 7400854980481283 };
            var allTasks = numbers.Select(n => Task.Run(async () =>
            {
                var result = await _fixture.Client.GetStringAsync($"api/values/isPrime?number={n}");
                Console.WriteLine($"{n} is a prime number? {result}");
            })).ToList();
            async Task ConcurrentApiRequests() => await Task.WhenAll(allTasks);
            var e = await Assert.ThrowsExceptionAsync<HttpRequestException>(ConcurrentApiRequests);
            Assert.AreEqual("Response status code does not indicate success: 429 (Too Many Requests).", e.Message);
        }

        [TestMethod]
        public async Task TestWithSemaphoreSlim()
        {
            var numbers = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 17, 19, 23, 29, 31, 41, 43, 1763, 7400854980481283 };

            var throttler = new SemaphoreSlim(2);
            await throttler.WaitAsync();
            var allTasks = numbers.Select(n => Task.Run(async () =>
            {
                try
                {
                    var result = await _fixture.Client.GetStringAsync($"api/values/isPrime?number={n}");
                    Console.WriteLine($"{n} is a prime number? {result}");
                }
                finally
                {
                    throttler.Release();
                }
            })).ToList();
            await Task.WhenAll(allTasks);
        }

        [TestMethod]
        public void CheckIpRateLimitOptions()
        {
            var options = _fixture.TestServer.Host.Services.GetRequiredService<IOptions<IpRateLimitOptions>>();
            Assert.AreEqual(1, options.Value.GeneralRules.Count);
            var generalRule = options.Value.GeneralRules[0];
            Assert.AreEqual("*:/api/*", generalRule.Endpoint);
            Assert.AreEqual("10s", generalRule.Period);
            Assert.AreEqual(2, generalRule.Limit);
        }
    }
}
