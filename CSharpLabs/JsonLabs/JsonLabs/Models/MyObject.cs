using System.Text.Json.Serialization;

namespace JsonLabs.Models
{
    public class MyObject
    {
        public int Id { get; set; }

        [JsonPropertyName("_full_name")]

        public string Name { get; set; }

        [JsonIgnore]    // without this attribute, then this property will be serialized to JSON.
        public string Extra { get; set; } = @"extra";

    }
}
