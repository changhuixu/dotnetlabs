using System.IO;
using System.Reflection;
using AsNoTrackingLabs.DbContext.EntityTypeConfigurations;
using AsNoTrackingLabs.Models;
using Microsoft.EntityFrameworkCore;

namespace AsNoTrackingLabs.DbContext
{
    public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public const string DbFileName = @"MyTodos.db";
        public DbSet<Todo> Todos { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = optionsBuilder
                .UseSqlite($"Data source={DbFileName}",
                    options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TodoEntityTypeConfigurations());
        }

        public static void EnsureDatabaseIsCleaned()
        {
            File.Delete(DbFileName);
        }
    }
}
