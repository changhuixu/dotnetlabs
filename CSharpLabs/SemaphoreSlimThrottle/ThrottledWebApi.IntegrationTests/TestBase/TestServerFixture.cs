using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace ThrottledWebApi.IntegrationTests.TestBase
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(@"..\..\..\..\ThrottledWebApi")
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  // otherwise, the TEST Server is not able to get configurations.
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            TestServer = new TestServer(builder); // https://github.com/aspnet/AspNetCore/issues/4545
            Client = TestServer.CreateClient();
            Console.WriteLine("TEST Server Started.");
        }

        public void Dispose()
        {
            Client.Dispose();
            TestServer.Dispose();
        }
    }
}
