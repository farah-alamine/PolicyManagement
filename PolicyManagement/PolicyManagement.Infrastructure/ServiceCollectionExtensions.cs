using Microsoft.Extensions.DependencyInjection;
using PolicyManagement.Core.Interfaces.Repositories;
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
            services.AddScoped<IPolicyTypeRepository, PolicyTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IPolicyService, PolicyService>();
            services.AddScoped<IPolicyTypeService, PolicyTypeService>();

            return services;
        }
    }
}
