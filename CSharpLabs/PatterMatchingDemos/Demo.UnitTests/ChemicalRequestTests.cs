using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.UnitTests
{
    [TestClass]
    public class ChemicalRequestTests
    {
        [DataTestMethod]
        [ChemicalRequestDataSource]
        public void ShouldGiveCorrectAction(string testName, ChemicalRequest request, bool acceptRequest)
        {
            Console.WriteLine(testName);

            var result1 = request.AcceptRequest();
            Assert.AreEqual(acceptRequest, result1);

            var result2 = request.ShouldAccept();
            Assert.AreEqual(acceptRequest, result2);
        }
    }

    public class ChemicalRequestDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[]
            {
                "row 1",
                new ChemicalRequest
                {
                    IsUserAuthorized = false
                },
                false
            };
            yield return new object[]
            {
                "row 2",
                new ChemicalRequest
                {
                    IsUserAuthorized = true,
                    IsChemicalAvailable = false
                },
                false
            };
            yield return new object[]
            {
                "row 3",
                new ChemicalRequest
                {
                    IsUserAuthorized = true,
                    IsChemicalAvailable = true,
                    IsChemicalHazardous = false
                },
                true
            };
            yield return new object[]
            {
                "row 4",
                new ChemicalRequest
                {
                    IsUserAuthorized = true,
                    IsChemicalAvailable = true,
                    IsChemicalHazardous = true,
                    IsRequesterTrained = false
                },
                false
            };
            yield return new object[]
            {
                "row 5",
                new ChemicalRequest
                {
                    IsUserAuthorized = true,
                    IsChemicalAvailable = true,
                    IsChemicalHazardous = true,
                    IsRequesterTrained = true
                },
                true
            };
            yield return new object[]
            {
                "row - ex1",
                new ChemicalRequest
                {
                    IsUserAuthorized = false,
                    IsChemicalAvailable = true,
                    IsChemicalHazardous = true,
                    IsRequesterTrained = true
                },
                false
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo.Name} - {data?[0]}";
        }
    }
}
