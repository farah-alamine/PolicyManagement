using Microsoft.Extensions.DependencyInjection;
using PolicyManagement.Core.Interfaces.IRepositories;
using PolicyManagement.Infrastructure.Repositories;
using PolicyManagement.Infrastructure.UnitOfWork;

namespace PolicyManagement.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>));

            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork. UnitOfWork>();

            return services;
        }
    }
}
