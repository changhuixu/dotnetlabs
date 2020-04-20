using System;
using UserRegistration.Models;

namespace UserRegistration
{
    internal class Program
    {
        private static void Main()
        {
            var user = new User("First Last", "SE", new DateTime(1987, 01, 29));

            var result = user.Register();

            Console.WriteLine(result);
        }
    }
}
