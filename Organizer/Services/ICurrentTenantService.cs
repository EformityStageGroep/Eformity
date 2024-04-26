namespace Organizer.Services
{
    public interface ICurrentTenantService
    {
        string? tenant_id { get; set; }
        public Task<bool> SetTenant(string tenant);
    }
}