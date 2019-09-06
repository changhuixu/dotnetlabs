using ConcurrencyControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurrencyControl.DbContext.Configurations
{
    internal class ConcurrentAccountWithTokenEntityTypeConfiguration : IEntityTypeConfiguration<ConcurrentAccountWithToken>
    {
        public void Configure(EntityTypeBuilder<ConcurrentAccountWithToken> builder)
        {
            builder.ToTable("ConcurrentAccountsWithToken");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasColumnType("money").IsConcurrencyToken();
        }
    }

    internal class ConcurrentAccountWithTokenEntityTypeConfigurationSqlite : IEntityTypeConfiguration<ConcurrentAccountWithToken>
    {
        public void Configure(EntityTypeBuilder<ConcurrentAccountWithToken> builder)
        {
            builder.ToTable("ConcurrentAccountsWithToken");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasConversion<double>().IsConcurrencyToken();
        }
    }
}
