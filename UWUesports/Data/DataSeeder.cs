using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Data
{
    public class DataSeeder
    {
        public static void SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UWUesportDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();

            try
            {
                context.Database.Migrate();

                if (context.Users.Any() || context.Organizations.Any())
                {
                    logger.LogInformation("Dane już istnieją, seed pominięty.");
                    return;
                }

                // === 1. Role ===
                /*var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Coach" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Player" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();*/

                // === 2. Organizacje ===
                var organizations = new List<Organization>
                {
                    new Organization { Name = "Esports United" },
                    new Organization { Name = "ProGaming Org" }
                };
                context.Organizations.AddRange(organizations);
                context.SaveChanges();

                // === 3. Użytkownicy ===
                var users = new List<ApplicationUser >
                {
                    new ApplicationUser  { Nickname = "PogChamp", Email = "pogchamp@example.com" },
                    new ApplicationUser  { Nickname = "UwUCat", Email = "uwucat@example.com" },
                    new ApplicationUser  { Nickname = "AimMaster", Email = "aimmaster@example.com" },
                    new ApplicationUser  { Nickname = "ProGamer", Email = "progamer@example.com" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // === 4. Role w organizacji ===
                /*var roleAssignments = new List<UserRoleAssignment>
                {
                    new UserRoleAssignment { UserId = users[0].Id, OrganizationId = organizations[0].Id, RoleId = roles.Single(r => r.Name == "Player").Id },
                    new UserRoleAssignment { UserId = users[1].Id, OrganizationId = organizations[0].Id, RoleId = roles.Single(r => r.Name == "Coach").Id },
                    new UserRoleAssignment { UserId = users[2].Id, OrganizationId = organizations[0].Id, RoleId = roles.Single(r => r.Name == "Player").Id },
                    new UserRoleAssignment { UserId = users[3].Id, OrganizationId = organizations[1].Id, RoleId = roles.Single(r => r.Name == "Admin").Id }
                };
                context.UserRoleAssignments.AddRange(roleAssignments);
                context.SaveChanges();
                */
                // === 5. Drużyny ===
                var teams = new List<Team>
                {
                    new Team { Name = "UwUGamers", OrganizationId = organizations[0].Id },
                    new Team { Name = "KawaiiKillers", OrganizationId = organizations[0].Id },
                    new Team { Name = "EliteSquad", OrganizationId = organizations[1].Id }
                };
                context.Teams.AddRange(teams);
                context.SaveChanges();

                // === 6. MembershipController (Użytkownicy w drużynach) ===
                var memberships = new List<Membership>
                {
                    new Membership { TeamId = teams[0].Id, UserId = users[0].Id },
                    new Membership { TeamId = teams[0].Id, UserId = users[1].Id },
                    new Membership { TeamId = teams[1].Id, UserId = users[1].Id },
                    new Membership { TeamId = teams[1].Id, UserId = users[2].Id },
                    new Membership { TeamId = teams[2].Id, UserId = users[3].Id }
                };
                context.Membership.AddRange(memberships);
                context.SaveChanges();

                logger.LogInformation("Dane testowe zostały poprawnie dodane do bazy danych.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Błąd podczas seedowania bazy danych.");
                throw;
            }
        }
    }
}
