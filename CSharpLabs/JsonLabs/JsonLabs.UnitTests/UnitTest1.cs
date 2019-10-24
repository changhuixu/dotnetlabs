using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using JsonLabs.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonLabs.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonString = File.ReadAllText("my-model.json");
            var jsonModel = JsonSerializer.Deserialize<MyModel>(jsonString, options);

            Assert.AreEqual("Hello World", jsonModel.MyString);
            Assert.AreEqual(123, jsonModel.MyInt);
            Assert.AreEqual(true, jsonModel.MyBoolean);
            Assert.AreEqual(12.3m, jsonModel.MyDecimal);
            Assert.AreEqual(new DateTime(2019, 9, 23, 10, 10, 0), jsonModel.MyDateTime1);
            Assert.AreEqual(new DateTime(2019, 9, 23), jsonModel.MyDateTime2);
            CollectionAssert.AreEquivalent(new List<string> { "apple", "banana", "orange" }, jsonModel.MyStringList);
            var a = new Dictionary<string, Person>
            {
                {"person1", new Person {Id = 201, Name = "C#"}},
                {"person2", new Person {Id = 302, Name = "F#"}}
            }.ToList();
            var b = jsonModel.MyDictionary.ToList();
            Assert.AreEqual(a.Count, b.Count);
            CollectionAssert.AreEquivalent(new List<string> {"person1", "person2"}, jsonModel.MyDictionary.Keys );
            Assert.AreEqual("Hi there", jsonModel.MyAnotherModel.MyString);
            Assert.AreEqual(456, jsonModel.MyAnotherModel.MyInt);
        }
    }
}
