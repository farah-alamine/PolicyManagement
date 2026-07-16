using Microsoft.Extensions.DependencyInjection;
using PolicyManagement.Core.Interfaces.IRepositories;
using PolicyManagement.Core.UnitOfWork;
using PolicyManagement.Infrastructure.Repositories;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Services;

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
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IPolicyService, PolicyService>();

            return services;
        }
    }
}
