using System;
using System.Drawing;
using Colors.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Colors.UnitTests
{
    [TestClass]
    public class ColorServicesTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var color = Color.FromArgb(100, 12, 134);
            Console.WriteLine(color.A); //255
            Console.WriteLine(color.R); //100
            Console.WriteLine(color.G); //12
            Console.WriteLine(color.B); //134
            Console.WriteLine(color);   //Color[A = 255, R = 100, G = 12, B = 134]
            Console.WriteLine(color.GetBrightness());   //0.28627452
            Console.WriteLine(color.GetHashCode());     //1754876516
            Console.WriteLine(color.GetHue());          //283.2787
            Console.WriteLine(color.GetSaturation());   //0.8356164

            color = ColorTranslator.FromHtml("#00FF00");
            Console.WriteLine(color.A); //255
            Console.WriteLine(color.R); //0
            Console.WriteLine(color.G); //255
            Console.WriteLine(color.B); //0
            Console.WriteLine(color);   //Color [A=255, R=0, G=255, B=0]
            Console.WriteLine(color.GetBrightness());   //0.5
            Console.WriteLine(color.GetHashCode());     //-1019428136
            Console.WriteLine(color.GetHue());          //120
            Console.WriteLine(color.GetSaturation());   //1

            Console.WriteLine(ColorTranslator.ToHtml(color));   //#00FF00
        }

        [TestMethod]
        public void GetLuminanceTests()
        {
            // the relative brightness of any point in a color space, normalized to 0 for darkest black and 1 for lightest white
            Assert.AreEqual(0, ColorServices.GetLuminance(Color.Black));
            Assert.AreEqual(1, ColorServices.GetLuminance(Color.White));
            Assert.AreEqual(0.2126, ColorServices.GetLuminance(Color.Red));
            Assert.AreEqual(0.48170267036309633, ColorServices.GetLuminance(Color.Orange));
            Assert.AreEqual(0.9278, ColorServices.GetLuminance(Color.Yellow));
            Assert.AreEqual(0.7152, ColorServices.GetLuminance(Color.FromArgb(255, 0, 255, 0)));
            Assert.AreEqual(0.0722, ColorServices.GetLuminance(Color.Blue));
            Assert.AreEqual(0.031075614863369856, ColorServices.GetLuminance(Color.Indigo));
            Assert.AreEqual(0.40315452986676326, ColorServices.GetLuminance(Color.Violet));
        }

        [TestMethod]
        public void GetColorContrastTests()
        {
            Assert.AreEqual(1.0, ColorServices.GetContrastRatio(Color.White, Color.White));
            Assert.AreEqual(21.0, ColorServices.GetContrastRatio(Color.Black, Color.White));
            // Pure red(#FF0000) has a ratio of 4:1. I am red text.
            // Pure green (#00FF00) has a very low ratio of 1.4:1. I am green text.
            // Pure blue (#0000FF) has a contrast ratio of 8.6:1.I am blue text.
            Assert.AreEqual(3.998, ColorServices.GetContrastRatio(Color.Red, Color.White));
            Assert.AreEqual(1.372, ColorServices.GetContrastRatio(Color.FromArgb(255, 0, 255, 0), Color.White));
            Assert.AreEqual(8.592, ColorServices.GetContrastRatio(Color.Blue, Color.White));

            Assert.AreEqual(1.0, ColorServices.GetContrastRatio(ColorTranslator.FromHtml("#a"), ColorTranslator.FromHtml("#b")));
        }

        [DataTestMethod]
        [DataRow("#FFFFFF", 255)]
        [DataRow("#33FF33", 170.748)]
        [DataRow("#333333", 51)]
        [DataRow("#000000", 0)]
        public void GetColorBrightnessTests(string hex, double brightness)
        {
            var color = ColorTranslator.FromHtml(hex);
            Assert.AreEqual(brightness, ColorServices.GetBrightness(color));
        }
    }
}
