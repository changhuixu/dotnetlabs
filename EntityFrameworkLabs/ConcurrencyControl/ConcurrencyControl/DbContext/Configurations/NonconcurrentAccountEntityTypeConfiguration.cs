using ConcurrencyControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurrencyControl.DbContext.Configurations
{
    internal class NonconcurrentAccountEntityTypeConfigurationSqlite : IEntityTypeConfiguration<NonconcurrentAccount>
    {
        public void Configure(EntityTypeBuilder<NonconcurrentAccount> builder)
        {
            builder.ToTable("NonconcurrentAccounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasConversion<double>();
        }
    }

    internal class NonconcurrentAccountEntityTypeConfiguration : IEntityTypeConfiguration<NonconcurrentAccount>
    {
        public void Configure(EntityTypeBuilder<NonconcurrentAccount> builder)
        {
            builder.ToTable("NonconcurrentAccounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasColumnType("money");
        }
    }
}
