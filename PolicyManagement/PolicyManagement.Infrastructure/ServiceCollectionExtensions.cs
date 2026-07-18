using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Repositories;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Services;
using PolicyManagement.Core.UnitOfWork;
using PolicyManagement.Infrastructure.Repositories;
using PolicyManagement.Infrastructure.Services;

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
            services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
