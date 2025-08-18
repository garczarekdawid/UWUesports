using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Models.ViewModels
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }
        public List<ApplicationUser> PlayersInTeam { get; set; }
        public List<ApplicationUser> AvailablePlayers { get; set; }
    }
}
