using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StringOperations.DbContext;
using StringOperations.Models;

namespace StringOperations
{
    internal class Program
    {
        private static void Main()
        {
            InitializeAndSeedDatabase();
            Console.WriteLine("========================================================================\r\n");

            using var dbContext = new MyDbContext();

            //var s0 = dbContext.Customers.Where(x=> x.LastName.StartsWith("pe", StringComparison.CurrentCultureIgnoreCase)).ToList();
            var aCustomers = dbContext.Customers.Where(x => x.LastName.StartsWith("pe")).ToList();
            Logger.LogInformation(string.Join("\t", aCustomers.Select(x => x.LastName)));
            Console.WriteLine();
            var bCustomers = dbContext.Customers.Where(x => x.LastName.ToLower().StartsWith("pe")).ToList();
            Logger.LogInformation(string.Join("\t", bCustomers.Select(x => x.LastName)));

            var cCustomers = dbContext.Customers.Where(x => x.LastName.StartsWith("Pe")).ToList();
            Logger.LogInformation(string.Join("\t", cCustomers.Select(x => x.LastName)));
            Console.WriteLine();

        }

        private static void InitializeAndSeedDatabase()
        {
            MyDbContext.EnsureDatabaseIsCleaned();
            using var dbContext = new MyDbContext();
            dbContext.Database.Migrate();

            var seed = JsonSerializer.Deserialize<Customer[]>(
                File.ReadAllText(Path.Combine(@"customers.json")),
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            dbContext.Customers.AddRange(seed);
            dbContext.SaveChanges();
        }
    }
}
