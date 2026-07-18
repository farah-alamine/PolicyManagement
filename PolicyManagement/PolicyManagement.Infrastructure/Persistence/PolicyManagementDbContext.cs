using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Infrastructure.Persistence
{
    public class PolicyManagementDbContext : DbContext
    {
        public PolicyManagementDbContext(
        DbContextOptions<PolicyManagementDbContext> options)
        : base(options)
        {
        }

        public DbSet<Policy> Policies => Set<Policy>();

        public DbSet<PolicyType> PolicyTypes => Set<PolicyType>();

        public DbSet<PolicyMember> PolicyMembers => Set<PolicyMember>();

        public DbSet<Claim> Claims => Set<Claim>();

        public DbSet<AppUser> AppUsers => Set<AppUser>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(user => user.Id);

                entity.Property(user => user.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(user => user.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasIndex(user => user.Email)
                    .IsUnique();

                entity.Property(user => user.PasswordHash)
                    .IsRequired();

                entity.Property(user => user.Role)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(user => user.IsActive)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(PolicyManagementDbContext).Assembly);
        }
        private void SetAuditFields()
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(entry =>
                    entry.State == EntityState.Added ||
                    entry.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var currentTime = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.RecordGuid =
                        entry.Entity.RecordGuid == Guid.Empty
                            ? Guid.NewGuid()
                            : entry.Entity.RecordGuid;

                    entry.Entity.CreatedDate = currentTime;

                    entry.Entity.CreatedBy ??= "System";
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdatedDate = currentTime;

                    entry.Entity.LastUpdatedBy ??= "System";

                    entry.Property(x => x.CreatedDate).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Property(x => x.RecordGuid).IsModified = false;
                }
            }
        }
    }
}