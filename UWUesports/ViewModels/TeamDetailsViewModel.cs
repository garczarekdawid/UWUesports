using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }
        public List<User> PlayersInTeam { get; set; }
        public List<User> AvailablePlayers { get; set; }
    }
}
