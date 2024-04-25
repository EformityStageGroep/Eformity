﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the many-to-many relationship
            modelBuilder.Entity<UserTeam>()
                .HasKey(ut => new { ut.User_Id, ut.Team_Id });

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.Users_Teams)
                .HasForeignKey(ut => ut.User_Id);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.Users_Teams)
                .HasForeignKey(ut => ut.Team_Id);
        }
    }
}