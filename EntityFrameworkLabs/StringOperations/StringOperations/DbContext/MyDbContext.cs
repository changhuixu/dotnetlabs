using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StringOperations.Models;

namespace StringOperations.DbContext
{
    public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public const string DbFileName = @"customers.db";
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
            });
        public DbSet<Customer> Customers { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlite($"Data source={DbFileName}",
                    options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().HasKey(x => x.Uuid);
            modelBuilder.Entity<Customer>().Property(x => x.Uuid).ValueGeneratedNever();
        }

        public static void EnsureDatabaseIsCleaned()
        {
            File.Delete(DbFileName);
        }
    }
}
