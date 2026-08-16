using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD.MultiTenant
{
    public static class MultiTenantServiceCollectionExtensions
    {
        public static IServiceCollection AddMultiTenant(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind tenants and register store as singleton
            services.AddSingleton<ITenantsStore>(sp => new TenantsStore(configuration));

            return services;
        }
    }
}
