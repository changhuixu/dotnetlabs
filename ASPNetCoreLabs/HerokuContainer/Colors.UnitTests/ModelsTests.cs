using System.Drawing;
using Colors.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Colors.UnitTests
{
    [TestClass]
    public class ModelsTests
    {
        [DataTestMethod]
        [DataRow("#FFFFFF", "#000000", 255, true)]
        [DataRow("#000000", "#FFFFFF", 255, true)]
        [DataRow("#33FF33", "#333333", 119.748, false)]
        [DataRow("#333333", "#33FF33", 119.748, false)]
        public void ShouldCorrectlyComputeColorBrightnessDifference(string hex1, string hex2, double diff, bool acceptable)
        {
            var color1 = ColorTranslator.FromHtml(hex1);
            var color2 = ColorTranslator.FromHtml(hex2);
            var brightnessDiff = new ColorBrightnessDifference(color1, color2);
            Assert.AreEqual(diff, brightnessDiff.Diff);
            Assert.AreEqual(acceptable, brightnessDiff.Acceptable);
        }


        [DataTestMethod]
        [DataRow("#FFFFFF", "#000000", 765, true)]
        [DataRow("#000000", "#FFFFFF", 765, true)]
        [DataRow("#33FF33", "#333333", 204, false)]
        [DataRow("#333333", "#33FF33", 204, false)]
        public void ShouldCorrectlyComputeColorDifference(string hex1, string hex2, double diff, bool acceptable)
        {
            var color1 = ColorTranslator.FromHtml(hex1);
            var color2 = ColorTranslator.FromHtml(hex2);
            var colorDifference = new ColorDifference(color1, color2);
            Assert.AreEqual(diff, colorDifference.Diff);
            Assert.AreEqual(acceptable, colorDifference.Acceptable);
        }
    }
}
