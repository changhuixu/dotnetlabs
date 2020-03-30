using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.IntegrationTests
{
    [TestClass]
    public class ConfigurationsTests
    {
        [TestMethod]
        public void CheckConfigurations()
        {
            var configurations = new TestHostFixture().ServiceProvider.GetRequiredService<IConfiguration>();
            Assert.AreEqual("MyValue1", configurations["MyKey1"]);
            Assert.AreEqual("MyValue2", configurations["MyKey2"]);
        }
    }
}
