using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.MultiTenant
{
    public class TenantsStore : ITenantsStore
    {
        public IReadOnlyDictionary<string, TenantInfo> Tenants { get; }

        public TenantsStore(IConfiguration configuration)
        {
            var section = configuration.GetSection("Tenants");
            var tenants = section.Get<TenantInfo[]>() ?? new TenantInfo[0];
            Tenants = tenants.ToDictionary(t => t.Id);
        }

        public bool TryGetTenant(string id, out TenantInfo tenant)
        {
            if (id != null && Tenants.TryGetValue(id, out tenant))
                return true;

            tenant = null;
            return false;
        }
    }
}
