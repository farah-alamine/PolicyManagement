using PolicyManagement.API.Middlewares;

namespace PolicyManagement.API.Extensions
{
    public static class TenantResolutionMiddlewareExtensions
    {
        public static IApplicationBuilder
            UseTenantResolution(
                this IApplicationBuilder app)
        {
            return app.UseMiddleware<
                TenantResolutionMiddleware>();
        }
    }
}
