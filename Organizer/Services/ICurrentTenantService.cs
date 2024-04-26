namespace Organizer.Services
{
    public interface ICurrentTenantService
    {
        string? TenantID { get; set; }
        public Task<bool> SetTenant(string tenant);
    }
}