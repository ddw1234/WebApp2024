using System.Collections.Generic;

namespace CRUD.MultiTenant
{
    public interface ITenantsStore
    {
        IReadOnlyDictionary<string, TenantInfo> Tenants { get; }
        bool TryGetTenant(string id, out TenantInfo tenant);
    }
}
