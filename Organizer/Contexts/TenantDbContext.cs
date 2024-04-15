using Microsoft.EntityFrameworkCore;
using Organizer.Entities;

namespace Organizer.Contexts
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) 
        { 

        }
        //net toegevoegd omdat organizer het ook heeft
        //
        public TenantDbContext()
        {
        }


        public DbSet<Tenant> Tenants { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Organizer.Properties.Resources.ConnectionString);
        }
    }
}
