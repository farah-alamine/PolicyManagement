using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PolicyManagement.Infrastructure.Persistence
{
    public class PolicyManagementDbContextFactory
       : IDesignTimeDbContextFactory<PolicyManagementDbContext>
    {
        public PolicyManagementDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable(
                    "TENANT_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString =
                    "Server=.\\SQLEXPRESS;" +
                    "Database=PolicyManagementDb;" +
                    "Trusted_Connection=True;" +
                    "Encrypt=True;" +
                    "TrustServerCertificate=True;";
            }

            var optionsBuilder =
                new DbContextOptionsBuilder<
                    PolicyManagementDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new PolicyManagementDbContext(
                optionsBuilder.Options);
        }
    }
}
