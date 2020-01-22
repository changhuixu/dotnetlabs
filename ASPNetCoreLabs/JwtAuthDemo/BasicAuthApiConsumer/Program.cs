using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthApiConsumer
{
    internal class Program
    {
        private static async Task Main()
        {
            const string url = @"https://localhost:44389";
            const string userName = @"admin";
            const string password = @"p@s5w0rd";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                AuthenticationSchemes.Basic.ToString(),
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"))
                );
            var response = await httpClient.GetAsync($"{url}/api/values/basic");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }
}
