using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;

namespace UWUesports.Web.Data
{
    public class UWUesportDbContext : DbContext
    {
        public UWUesportDbContext(DbContextOptions<UWUesportDbContext> options): base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamPlayer>()
                .HasKey(tp => new { tp.TeamId, tp.UserId  });

            modelBuilder.Entity<TeamPlayer>()
                .HasOne(tp => tp.Team)
                .WithMany(t => t.TeamPlayers)
                .HasForeignKey(tp => tp.TeamId);

            modelBuilder.Entity<TeamPlayer>()
                .HasOne(tp => tp.User)
                .WithMany(p => p.TeamPlayers)
                .HasForeignKey(tp => tp.UserId );

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Teams)
                .WithOne(t => t.Organization)
                .HasForeignKey(t => t.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Organization)
                .WithMany(o => o.Teams)
                .HasForeignKey(t => t.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
