using System;

namespace StringOperations.Models
{
    public class Customer
    {
        public string Uuid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public int Seniority { get; set; }
    }
}
