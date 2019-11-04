using AsNoTrackingLabs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsNoTrackingLabs.DbContext.EntityTypeConfigurations
{
    internal class TodoEntityTypeConfigurations : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Todos");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.TaskName).HasColumnName("TaskName").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Completed).HasColumnName("Completed").IsRequired().HasDefaultValue(false);
        }
    }
}
