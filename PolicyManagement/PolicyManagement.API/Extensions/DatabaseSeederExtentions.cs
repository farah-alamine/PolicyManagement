using Microsoft.AspNetCore.Identity;
using PolicyManagement.Core.Entities;
using PolicyManagement.Infrastructure.Persistence;

namespace PolicyManagement.API.Extensions
{
    public static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(
            this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<AuthenticationDbContext>();

            var passwordHasher = scope.ServiceProvider
                .GetRequiredService<IPasswordHasher<AppUser>>();

            await DatabaseSeeder.SeedAsync(
                dbContext,
                passwordHasher);
        }
    }
}
