using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Infrastructure.Persistence
{

    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(
            DbContextOptions<AuthenticationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers => Set<AppUser>();

        public DbSet<Tenant> Tenants => Set<Tenant>();

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SetAuditFields();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenants");

                entity.HasKey(tenant => tenant.Id);

                entity.Property(tenant => tenant.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(tenant => tenant.Identifier)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(tenant => tenant.Identifier)
                    .IsUnique();

                entity.Property(tenant => tenant.ConnectionString)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(tenant => tenant.IsActive)
                    .IsRequired();
            });

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

                
                entity.HasIndex(user => new
                {
                    user.TenantId,
                    user.Email
                }).IsUnique();

                entity.Property(user => user.PasswordHash)
                    .IsRequired();

                entity.Property(user => user.Role)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(user => user.IsActive)
                    .IsRequired();

                entity.HasOne(user => user.Tenant)
                    .WithMany(tenant => tenant.Users)
                    .HasForeignKey(user => user.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
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

                    entry.Property(x => x.CreatedDate)
                        .IsModified = false;

                    entry.Property(x => x.CreatedBy)
                        .IsModified = false;

                    entry.Property(x => x.RecordGuid)
                        .IsModified = false;
                }
            }
        }
    }
}
