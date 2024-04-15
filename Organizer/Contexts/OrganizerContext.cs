using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Graph.CallRecords;
using Organizer.Entities;
using Organizer.Services;
using System.Net.Http.Headers;
using System.Reflection;

namespace Organizer.Contexts
{
    public class OrganizerContext : DbContext
    {
        private readonly ICurrentTenantService _currentTenantService;
        public string CurrentTenantId { get; set; }
        public OrganizerContext(DbContextOptions<OrganizerContext> options, ICurrentTenantService currentTenantService) : base(options)
        {
           _currentTenantService = currentTenantService;
            CurrentTenantId = _currentTenantService.TenantId;
        }

        public OrganizerContext()
        {
        }


        public DbSet<User> Users { get; set; }
        
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Organizer.Entities.Task> Task { get; set; }


        // on app startup
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Entities.Task>().HasQueryFilter(a => a.TenantId == CurrentTenantId);
        }



        // every time we save something
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = CurrentTenantId;
                        break;
                }
            }
            var result = base.SaveChanges();
            return result;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Organizer.Properties.Resources.ConnectionString);



        }

    }
}