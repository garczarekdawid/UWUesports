using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;

namespace UWUesports.Web.Data
{
    public class DataSeeder
    {
        public static void SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UWUesportDbContext>();

            // Automatyczne tworzenie bazy i migracji
            context.Database.Migrate();

            // Sprawdź, czy dane już istnieją
            if (context.Teams.Any() || context.Players.Any())
                return;

            // Gracze
            var p1 = new Player { Nickname = "PogChamp" };
            var p2 = new Player { Nickname = "UwUCat" };
            var p3 = new Player { Nickname = "AimMaster" };

            // Drużyny
            var t1 = new Team { Name = "UwUGamers" };
            var t2 = new Team { Name = "KawaiiKillers" };

            // Relacje wiele-do-wielu (TeamPlayer)
            var tp1 = new TeamPlayer { Team = t1, Player = p1 };
            var tp2 = new TeamPlayer { Team = t1, Player = p2 };
            var tp3 = new TeamPlayer { Team = t2, Player = p2 };
            var tp4 = new TeamPlayer { Team = t2, Player = p3 };

            context.AddRange(tp1, tp2, tp3, tp4);
            context.SaveChanges();
        }
    }
}
