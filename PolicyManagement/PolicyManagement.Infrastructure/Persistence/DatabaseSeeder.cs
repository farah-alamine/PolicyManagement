using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Entities;

namespace PolicyManagement.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
     AuthenticationDbContext dbContext,
     IPasswordHasher<AppUser> passwordHasher)
        {
            const string adminEmail = "admin@policymanagement.com";

            var adminExists = await dbContext.AppUsers
                .AnyAsync(user => user.Email == adminEmail);

            if (adminExists)
            {
                return;
            }

            var tenant = await dbContext.Tenants
                .FirstOrDefaultAsync(x => x.Identifier == "tenant-a");

            if (tenant is null)
            {
                tenant = new Tenant
                {
                    RecordGuid = Guid.NewGuid(),
                    Name = "Default Tenant",
                    Identifier = "tenant-a",
                    ConnectionString =
                        "Server=.\\SQLEXPRESS;" +
                        "Database=PolicyManagementDb;" +
                        "Trusted_Connection=True;" +
                        "Encrypt=True;" +
                        "TrustServerCertificate=True;",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                await dbContext.Tenants.AddAsync(tenant);
                await dbContext.SaveChangesAsync();
            }

            var adminUser = new AppUser
            {
                RecordGuid = Guid.NewGuid(),
                FullName = "System Administrator",
                Email = adminEmail,
                Role = "Admin",
                IsActive = true,
                TenantId = tenant.Id,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System"
            };

            adminUser.PasswordHash =
                passwordHasher.HashPassword(
                    adminUser,
                    "Admin@123");

            await dbContext.AppUsers.AddAsync(adminUser);
            await dbContext.SaveChangesAsync();
        }
    }
}
