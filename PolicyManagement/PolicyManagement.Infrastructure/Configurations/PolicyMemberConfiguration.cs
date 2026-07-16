using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Core.Entities;

namespace PolicyManagement.Infrastructure.Configurations
{
    public class PolicyMemberConfiguration : IEntityTypeConfiguration<PolicyMember>
    {
        public void Configure(EntityTypeBuilder<PolicyMember> builder)
        {
            builder.ToTable("PolicyMembers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RecordGuid)
                .IsRequired();

            builder.HasIndex(x => x.RecordGuid)
                .IsUnique();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.Property(x => x.RelationshipToPolicyHolder)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastUpdatedBy)
                .HasMaxLength(100);
        }
    }
}