using Microsoft.EntityFrameworkCore;

namespace LockStatementLab
{
    public class MyDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; protected set; }

        public MyDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}