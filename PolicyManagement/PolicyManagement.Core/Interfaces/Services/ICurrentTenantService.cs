namespace PolicyManagement.Core.Interfaces.Services
{

    public interface ICurrentTenantService
    {
        Guid TenantGuid { get; }

        string Identifier { get; }

        string ConnectionString { get; }

        bool IsResolved { get; }

        void SetTenant(Guid tenantGuid, string identifier, string connectionString);
    }
}
