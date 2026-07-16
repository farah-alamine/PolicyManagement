using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Core.Entities;

namespace PolicyManagement.Infrastructure.Configurations
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claims");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RecordGuid)
                .IsRequired();

            builder.HasIndex(x => x.RecordGuid)
                .IsUnique();

            builder.Property(x => x.ClaimNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.ClaimNumber)
                .IsUnique();

            builder.Property(x => x.ClaimDate)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastUpdatedBy)
                .HasMaxLength(100);
        }
    }
}
