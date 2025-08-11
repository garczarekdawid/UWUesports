using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }
        public List<ApplicationUser > PlayersInTeam { get; set; }
        public List<ApplicationUser > AvailablePlayers { get; set; }
    }
}
