using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyManagement.Infrastructure.Persistence
{

    public class AuthenticationDbContextFactory
        : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(
            string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<AuthenticationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=PolicyManagementAuthDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");

            return new AuthenticationDbContext(
                optionsBuilder.Options);
        }
    }
}
