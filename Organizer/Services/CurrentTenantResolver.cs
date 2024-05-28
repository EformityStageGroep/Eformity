using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;

namespace Organizer.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly TenantDbContext _context;

        public CurrentTenantService(TenantDbContext context)
        {
            _context = context;
        }

        public string? tenantid { get; set; }

        public async Task<bool> SetTenant(string tenant)
        {
            var tenantInfo = await _context.Tenants.Where(x => x.id == tenant).FirstOrDefaultAsync();
            if (tenantInfo != null)
            {
                tenantid = tenantInfo.id;
                return true;
            }
            else
            {
                throw new Exception("Tenant invalid");
            }
        }
    }
}