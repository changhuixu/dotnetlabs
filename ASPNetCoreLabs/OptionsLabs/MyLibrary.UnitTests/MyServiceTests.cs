using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyLibrary.UnitTests
{
    [TestClass]
    public class MyServiceTests
    {
        [TestMethod]
        public void DemoTest()
        {
            var svc = new MyService(NullLogger<MyService>.Instance,
                Options.Create(new MyServiceOptions
                {
                    Option1 = "say hello",
                    Option2 = true
                }));
            svc.DoWork();
        }
    }
}
