using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgeCalculator.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [DataRow("20120101", "20170505", 5)]
        [DataRow("20120229", "20170301", 5)]
        [DataRow("20120229", "20170228", 4)]
        [DataRow("20120229", "20170227", 4)]
        [DataRow("20120229", "20200228", 7)]
        [DataRow("20120229", "20200229", 8)]
        [DataTestMethod]
        public void ShouldCalculateAge(string dob, string today, int age)
        {
            var birthday = DateTime.ParseExact(dob, "yyyyMMdd", CultureInfo.InvariantCulture);
            var now = DateTime.ParseExact(today, "yyyyMMdd", CultureInfo.InvariantCulture);
            if (age == 8)
            {
                
            }
            var result = AgeCalculator.Calculate(birthday, now);
            Assert.AreEqual(age, result);
        }
    }
}
