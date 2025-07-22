using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UWUesports.Web.Models;

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

                if (context.Teams.Any() || context.Users.Any() || context.Organizations.Any())
                {
                    logger.LogInformation("Dane już istnieją, seed pominięty.");
                    return;
                }

                // Dodaj organizacje
                var organizations = new List<Organization>
                {
                    new Organization { Name = "Esports United" },
                    new Organization { Name = "ProGaming Org" }
                };
                context.Organizations.AddRange(organizations);
                context.SaveChanges();

                // Dodaj użytkowników
                var users = new List<User>
                {
                    new User { Nickname = "PogChamp", Email = "pogchamp@example.com", Role = "User" },
                    new User { Nickname = "UwUCat", Email = "uwucat@example.com", Role = "Coach" },
                    new User { Nickname = "AimMaster", Email = "aimmaster@example.com", Role = "User" },
                    new User { Nickname = "ProGamer", Email = "progamer@example.com", Role = "Admin" }
                };
                context.Users.AddRange(users);

                // Dodaj drużyny z przypisaniem do organizacji
                var teams = new List<Team>
                {
                    new Team { Name = "UwUGamers", OrganizationId = organizations[0].Id },
                    new Team { Name = "KawaiiKillers", OrganizationId = organizations[0].Id },
                    new Team { Name = "EliteSquad", OrganizationId = organizations[1].Id }
                };
                context.Teams.AddRange(teams);

                context.SaveChanges();

                // Powiązania TeamPlayer
                var teamPlayers = new List<TeamPlayer>
                {
                    new TeamPlayer { TeamId = teams[0].Id, UserId = users[0].Id },
                    new TeamPlayer { TeamId = teams[0].Id, UserId = users[1].Id },
                    new TeamPlayer { TeamId = teams[1].Id, UserId = users[1].Id },
                    new TeamPlayer { TeamId = teams[1].Id, UserId = users[2].Id },
                    new TeamPlayer { TeamId = teams[2].Id, UserId = users[3].Id }
                };
                context.TeamPlayers.AddRange(teamPlayers);

                context.SaveChanges();
                logger.LogInformation("Dane testowe z organizacjami zostały dodane do bazy danych.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Błąd podczas seedowania bazy danych.");
                throw;
            }
        }
    }
}
