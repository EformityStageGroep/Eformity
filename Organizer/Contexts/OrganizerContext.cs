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
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> Users_Teams { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Console.WriteLine("testtttt");
            // Configuring the many-to-many relationship
            modelBuilder.Entity<UserTeam>()
                .HasKey(ut => new { ut.user_id, ut.team_id });

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.Users_Teams)
                .HasForeignKey(ut => ut.user_id);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.Users_Teams)
                .HasForeignKey(ut => ut.team_id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.role_id)
                .HasPrincipalKey(r => r.id);
        }
    }
}