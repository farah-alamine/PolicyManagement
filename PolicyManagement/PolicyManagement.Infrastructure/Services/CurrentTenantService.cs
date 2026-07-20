using PolicyManagement.Core.Interfaces.Services;

namespace PolicyManagement.Infrastructure.Services
{

    public class CurrentTenantService : ICurrentTenantService
    {
        public Guid TenantGuid { get; private set; }

        public string Identifier { get; private set; }
            = string.Empty;

        public string ConnectionString { get; private set; }
            = string.Empty;

        public bool IsResolved =>
            TenantGuid != Guid.Empty &&
            !string.IsNullOrWhiteSpace(ConnectionString);

        public void SetTenant(
            Guid tenantGuid,
            string identifier,
            string connectionString)
        {
            TenantGuid = tenantGuid;
            Identifier = identifier;
            ConnectionString = connectionString;
        }
    }
}
