using Microsoft.EntityFrameworkCore;
using Organizer.Entities;

namespace Organizer.Contexts
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext()
        {
           // Database.SetInitializer(new MigrateDatabaseToLatestVersion<OrganizerContext, Configuration>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Organizer.Properties.Resources.ConnectionString);

        

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Task> Task { get; set; }
    }
}