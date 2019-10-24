using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JsonLabs.Models;

namespace JsonLabs
{
    internal class Program
    {
        private static void Main()
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }; // https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=netcore-3.0


            // serialization/deserialization of most commonly seen object types
            var jsonString = File.ReadAllText("my-model.json");
            var jsonModel = JsonSerializer.Deserialize<MyModel>(jsonString, options);
            var modelJson = JsonSerializer.Serialize(jsonModel, options);
            Console.WriteLine(modelJson);
            Console.WriteLine();

            // JSON Property Name Attributes
            var objJson = File.ReadAllText("my-object.json");
            var objInstance = JsonSerializer.Deserialize<MyObject>(objJson, options);
            Console.WriteLine(JsonSerializer.Serialize(objInstance, options));
            Console.WriteLine();

            // serialization/deserialization of object arrays
            var objsJsonString = File.ReadAllText("my-objects.json");
            var myObjects = JsonSerializer.Deserialize<MyObject[]>(objsJsonString, options);
            Console.WriteLine(JsonSerializer.Serialize(myObjects, options));
            Console.WriteLine();

            // using JsonDocument to partially read JSON elements
            var jsonBytes = File.ReadAllBytes("my-model.json");
            using var jsonDoc = JsonDocument.Parse(jsonBytes);
            var root = jsonDoc.RootElement;

            var myString = root.GetProperty("myString").GetString();    // Get a string from a JsonElement
            Console.WriteLine(myString);

            var myInt = root.GetProperty("myInt").GetInt32();           // Get an integer from a JsonElement
            Console.WriteLine(myInt);

            var fruits = new List<string>();
            var myStringList = root.GetProperty("myStringList");        // Get a list from a JsonElement
            for (var i = 0; i < myStringList.GetArrayLength(); i++)
            {
                fruits.Add(myStringList[i].GetString());
            }
            Console.WriteLine(string.Join(", ", fruits));

            Console.ReadLine();
        }
    }
}
