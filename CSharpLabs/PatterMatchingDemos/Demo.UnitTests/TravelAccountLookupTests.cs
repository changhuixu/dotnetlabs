using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.UnitTests
{
    [TestClass]
    public class TravelAccountLookupTests
    {
        // In real-world application, we need to provide an exhausted list of test cases.

        [TestMethod]
        public void GroupTripTests()
        {
            var lookup = new TravelAccountLookup
            {
                GroupTravel = true,
                TravelerType = TravelerType.Other,
                TravelPurposeCode = "SR",
                Destination = "O"
            };
            Assert.AreEqual("6035", lookup.GetAccountNumber());
            Assert.AreEqual("6035", lookup.GetAccountNumber2());
        }

        [TestMethod]
        public void StudentTravelTests()
        {
            var lookup = new TravelAccountLookup
            {
                GroupTravel = false,
                TravelerType = TravelerType.Student,
                TravelPurposeCode = "ER",
                Destination = "I"
            };
            Assert.AreEqual("6030", lookup.GetAccountNumber());
            Assert.AreEqual("6030", lookup.GetAccountNumber2());
        }


        [TestMethod]
        public void OtherTravelTests()
        {
            var lookup = new TravelAccountLookup
            {
                GroupTravel = false,
                TravelerType = TravelerType.Other,
                TravelPurposeCode = "SR",
                Destination = "I"
            };
            Assert.AreEqual("6050", lookup.GetAccountNumber());
            Assert.AreEqual("6050", lookup.GetAccountNumber2());
        }
    }
}
