namespace Organizer.Services
{
    public interface ICurrentTenantService
    {
        string? tenantid { get; set; }
        public Task<bool> SetTenant(string tenant);
    }
}