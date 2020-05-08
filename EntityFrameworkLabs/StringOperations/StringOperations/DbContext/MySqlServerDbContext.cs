using Microsoft.EntityFrameworkCore;
using StringOperations.Models;

namespace StringOperations.DbContext
{
    public class MySqlServerDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Customer> Customers { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = optionsBuilder
                .UseLoggerFactory(Program.MyLoggerFactory)
                .UseSqlServer("Server=.;Database=TestDb;Trusted_Connection=True;Integrated Security=True;MultipleActiveResultSets=true;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().HasKey(x => x.Uuid);
            modelBuilder.Entity<Customer>().Property(x => x.Uuid).ValueGeneratedNever();
        }
    }
}
