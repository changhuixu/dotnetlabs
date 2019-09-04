using ConcurrencyControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurrencyControl.DbContext.Configurations
{
    internal class ConcurrentAccountWithRowVersionEntityTypeConfiguration : IEntityTypeConfiguration<ConcurrentAccountWithRowVersion>
    {
        public void Configure(EntityTypeBuilder<ConcurrentAccountWithRowVersion> builder)
        {
            builder.ToTable("ConcurrentAccountsWithRowVersion");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasColumnType("money");
            builder.Property(x => x.Timestamp).HasColumnName("Timestamp").IsRowVersion();
        }
    }

    internal class ConcurrentAccountWithRowVersionEntityTypeConfigurationSqlite : IEntityTypeConfiguration<ConcurrentAccountWithRowVersion>
    {
        public void Configure(EntityTypeBuilder<ConcurrentAccountWithRowVersion> builder)
        {
            builder.ToTable("ConcurrentAccountsWithRowVersion");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasConversion<double>();
            builder.Property(x => x.Timestamp).HasColumnName("Timestamp")
                .HasColumnType("BLOB")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRowVersion();
        }
    }
}
