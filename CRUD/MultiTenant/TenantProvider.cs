using System.Threading;

namespace CRUD.MultiTenant
{
    public class TenantProvider : ITenantProvider
    {
        private static readonly AsyncLocal<TenantInfo> _current = new AsyncLocal<TenantInfo>();

        public string TenantId => _current.Value?.Id;

        public TenantInfo CurrentTenant => _current.Value;

        public void SetTenant(TenantInfo tenant)
        {
            _current.Value = tenant;
        }
    }
}
