using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Infrastructure.Persistence;

namespace PolicyManagement.API.Middlewares
{

    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            AuthenticationDbContext authenticationDbContext,
            ICurrentTenantService currentTenantService)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                await _next(context);
                return;
            }

            var tenantGuidValue =
                context.User.FindFirst("tenant_guid")?.Value;

            if (
                string.IsNullOrWhiteSpace(tenantGuidValue) ||
                !Guid.TryParse(
                    tenantGuidValue,
                    out var tenantGuid))
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        message =
                            "The authentication token does not contain a valid tenant."
                    });

                return;
            }

            var tenant = await authenticationDbContext.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    currentTenant =>
                        currentTenant.RecordGuid == tenantGuid &&
                        currentTenant.IsActive,
                    context.RequestAborted);

            if (tenant is null)
            {
                context.Response.StatusCode =
                    StatusCodes.Status403Forbidden;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        message =
                            "The requested tenant is not available."
                    });

                return;
            }

            currentTenantService.SetTenant(
                tenant.RecordGuid,
                tenant.Identifier,
                tenant.ConnectionString);

            await _next(context);
        }
    }
}
