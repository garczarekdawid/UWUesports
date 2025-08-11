using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;

namespace UWUesports.Web.Data
{
    public class UWUesportDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public UWUesportDbContext(DbContextOptions<UWUesportDbContext> options)
            : base(options) { }

        public DbSet<Team> Teams { get; set; }
        // Usuń DbSet<User> Users, bo masz ApplicationUser jako użytkownika Identity
        public DbSet<Membership> TeamPlayers { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        // Możesz usunąć swój Role DbSet jeśli chcesz używać IdentityRole<int> albo zostawić jeśli masz własne rozszerzenia
        // Jeśli korzystasz z IdentityRole<int>, nie trzeba mieć osobnej tabeli Role
        // public DbSet<Role> Roles { get; set; }

        public DbSet<IdentityRole<int>> Roles { get; set; }

        public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Membership>()
                .HasKey(tp => new { tp.TeamId, tp.UserId });

            modelBuilder.Entity<Membership>()
                .HasOne(tp => tp.Team)
                .WithMany(t => t.TeamPlayers)
                .HasForeignKey(tp => tp.TeamId);

            modelBuilder.Entity<Membership>()
                .HasOne(tp => tp.User)
                .WithMany(p => p.TeamPlayers)
                .HasForeignKey(tp => tp.UserId);

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

            modelBuilder.Entity<UserRoleAssignment>()
                .HasKey(ura => new { ura.UserId, ura.OrganizationId, ura.RoleId });

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(ura => ura.User)
                .WithMany(u => u.RoleAssignments)
                .HasForeignKey(ura => ura.UserId);

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(ura => ura.Organization)
                .WithMany(o => o.UserRoles)
                .HasForeignKey(ura => ura.OrganizationId);

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(ura => ura.Role)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);
        }
    }
}
