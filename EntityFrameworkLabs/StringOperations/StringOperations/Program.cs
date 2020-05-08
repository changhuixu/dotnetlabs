using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StringOperations.DbContext;
using StringOperations.Models;

namespace StringOperations
{
    internal class Program
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information)
                    .AddConsole();
            });

        private static readonly ILogger Logger = MyLoggerFactory.CreateLogger<Program>();

        private static void Main()
        {
            //InitializeAndSeedSqliteDatabase();    // only need it for SQLite, the first time for seeding data
            using var dbContext = new MySqliteDbContext();

            //InitializeAndSeedSqlServerDatabase(); // only need it for SQL Server, the first time for seeding data
            //using var dbContext = new MySqlServerDbContext();

            Logger.LogInformation("\r\n========================================================================\r\n");

            //var s0 = dbContext.Customers.Where(x=> x.LastName.StartsWith("pe", StringComparison.CurrentCultureIgnoreCase)).ToList();
            var startsWith1 = dbContext.Customers.Where(x => x.LastName.StartsWith("pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", startsWith1));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var startsWith2 = dbContext.Customers.Where(x => x.LastName.ToLower().StartsWith("pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", startsWith2));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var startsWith3 = dbContext.Customers.Where(x => x.LastName.StartsWith("Pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", startsWith3));
            Logger.LogInformation("\r\n========================================================================\r\n");

            var contains1 = dbContext.Customers.Where(x => x.LastName.Contains("pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", contains1));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var contains2 = dbContext.Customers.Where(x => x.LastName.ToLower().Contains("pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", contains2));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var contains3 = dbContext.Customers.Where(x => x.LastName.Contains("Pe")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", contains3));
            Logger.LogInformation("\r\n========================================================================\r\n");

            var equals1 = dbContext.Customers.Where(x => x.LastName.Equals("perez")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals1));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var equals2 = dbContext.Customers.Where(x => x.LastName.ToLower().Equals("perez")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals2));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var equals3 = dbContext.Customers.Where(x => x.LastName.Equals("Perez")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals3));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var equals4 = dbContext.Customers.Where(x => x.LastName == "perez").Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals4));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var equals5 = dbContext.Customers.Where(x => x.LastName.ToLower() == "perez").Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals5));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var equals6 = dbContext.Customers.Where(x => x.LastName == "Perez").Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", equals6));
            Logger.LogInformation("\r\n========================================================================\r\n");

            Logger.LogInformation("\r\nLIKE Operator\r\n");
            var like1 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName, "pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like1));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var like2 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName.ToLower(), "pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like2));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var like3 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName, "Pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like3));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var like4 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName, "%pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like4));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var like5 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName.ToLower(), "%pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like5));
            Logger.LogInformation("\r\n========================================================================\r\n");
            var like6 = dbContext.Customers.Where(x => EF.Functions.Like(x.LastName, "%Pe%")).Select(x => x.LastName).ToList();
            Logger.LogInformation(string.Join("\t", like6));
            Logger.LogInformation("\r\n========================================================================\r\n");
        }

        private static void InitializeAndSeedSqliteDatabase()
        {
            MySqliteDbContext.EnsureDatabaseIsCleaned();
            using var dbContext = new MySqliteDbContext();
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

        private static void InitializeAndSeedSqlServerDatabase()
        {
            using var dbContext = new MySqlServerDbContext();

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
