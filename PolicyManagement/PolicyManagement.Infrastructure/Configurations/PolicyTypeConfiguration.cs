using PolicyManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PolicyManagement.Infrastructure.Configurations
{
    public class PolicyTypeConfiguration : IEntityTypeConfiguration<PolicyType>
    {
        public void Configure(EntityTypeBuilder<PolicyType> builder)
        {
            builder.ToTable("PolicyTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RecordGuid)
                .IsRequired();

            builder.HasIndex(x => x.RecordGuid)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastUpdatedBy)
                .HasMaxLength(100);
        }
    }
}
