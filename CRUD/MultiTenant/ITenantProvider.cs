namespace CRUD.MultiTenant
{
    public interface ITenantProvider
    {
        string TenantId { get; }
        TenantInfo CurrentTenant { get; }
        void SetTenant(TenantInfo tenant);
    }
}
