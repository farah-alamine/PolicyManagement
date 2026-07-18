using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyManagement.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
            PolicyManagementDbContext dbContext,
            IPasswordHasher<AppUser> passwordHasher)
        {
            const string adminEmail = "admin@policymanagement.com";

            var adminExists = await dbContext.AppUsers
                .AnyAsync(user => user.Email == adminEmail);

            if (adminExists)
            {
                return;
            }

            var adminUser = new AppUser
            {
                RecordGuid = Guid.NewGuid(),
                FullName = "System Administrator",
                Email = adminEmail,
                Role = "Admin",
                IsActive = true,
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
