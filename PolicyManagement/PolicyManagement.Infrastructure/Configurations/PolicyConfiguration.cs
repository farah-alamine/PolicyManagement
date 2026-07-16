using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Core.Entities;

namespace PolicyManagement.Infrastructure.Configurations
{
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policies");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RecordGuid)
                .IsRequired();

            builder.HasIndex(x => x.RecordGuid)
                .IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.EffectiveDate)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastUpdatedBy)
                .HasMaxLength(100);

            builder.HasOne(x => x.PolicyType)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Members)
                .WithOne(x => x.Policy)
                .HasForeignKey(x => x.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Claims)
                .WithOne(x => x.Policy)
                .HasForeignKey(x => x.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
