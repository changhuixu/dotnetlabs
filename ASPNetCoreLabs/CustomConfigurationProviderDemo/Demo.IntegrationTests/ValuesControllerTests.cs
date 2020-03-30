using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests
    {
        private readonly HttpClient _httpClient = new TestHostFixture().Client;

        [TestMethod]
        public async Task ShouldGetAllKeyValuePairs()
        {
            var response = await _httpClient.GetAsync("api/values");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("[\"MyValue1\",\"MyValue2\"]", result);
        }

        [DataTestMethod]
        [DataRow("MyKey1", "MyValue1")]
        [DataRow("MyKey2", "MyValue2")]
        public async Task TestWithDataSource(string key, string value)
        {
            var response = await _httpClient.GetAsync($"api/values/{key}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(value, result);
        }
    }
}
