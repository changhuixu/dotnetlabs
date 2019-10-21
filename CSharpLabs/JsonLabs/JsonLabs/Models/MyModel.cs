using System;
using System.Collections.Generic;

namespace JsonLabs.Models
{
    public class MyModel
    {
        public string MyString { get; set; }
        public int MyInt { get; set; }
        public bool MyBoolean { get; set; }
        public decimal MyDecimal { get; set; }
        public DateTime MyDateTime1 { get; set; }
        public DateTime MyDateTime2 { get; set; }
        public List<string> MyStringList { get; set; }
        public Dictionary<string, Person> MyDictionary { get; set; }
        public MyModel MyAnotherModel { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
