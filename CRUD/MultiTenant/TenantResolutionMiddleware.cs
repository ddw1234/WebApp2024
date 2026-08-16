using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CRUD.MultiTenant
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Resolve tenant id from header
            var tenantId = context.Request.Headers["X-Tenant-Id"].ToString();

            if (!string.IsNullOrEmpty(tenantId))
            {
                var provider = context.RequestServices.GetService(typeof(ITenantProvider)) as ITenantProvider;
                if (provider != null)
                {
                    provider.SetTenant(new TenantInfo { Id = tenantId });
                }
            }

            await _next(context);
        }
    }
}
