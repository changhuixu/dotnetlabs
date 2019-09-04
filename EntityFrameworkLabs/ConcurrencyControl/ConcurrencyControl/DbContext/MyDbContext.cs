using System.IO;
using System.Reflection;
using ConcurrencyControl.DbContext.Configurations;
using ConcurrencyControl.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyControl.DbContext
{
    public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public const string DbFileName = @"ConcurrencyControl.db";
        public DbSet<ConcurrentAccountWithToken> ConcurrentAccountsWithToken { get; protected set; }
        public DbSet<ConcurrentAccountWithRowVersion> ConcurrentAccountsWithRowVersion { get; protected set; }
        public DbSet<NonconcurrentAccount> NonconcurrentAccounts { get; protected set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder )
        {
                optionsBuilder = optionsBuilder.UseSqlite($"Data source={DbFileName}",
                    options => { options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName); });
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsSqlite())
            {
                modelBuilder.ApplyConfiguration(new ConcurrentAccountWithTokenEntityTypeConfigurationSqlite());
                modelBuilder.ApplyConfiguration(new ConcurrentAccountWithRowVersionEntityTypeConfigurationSqlite());
                modelBuilder.ApplyConfiguration(new NonconcurrentAccountEntityTypeConfigurationSqlite());
            }
            else
            {
                modelBuilder.ApplyConfiguration(new ConcurrentAccountWithTokenEntityTypeConfiguration());
                modelBuilder.ApplyConfiguration(new ConcurrentAccountWithRowVersionEntityTypeConfiguration());
                modelBuilder.ApplyConfiguration(new NonconcurrentAccountEntityTypeConfiguration());
            }
        }

        public static void EnsureDatabaseIsCleaned()
        {
            File.Delete(DbFileName);
        }
    }
}
