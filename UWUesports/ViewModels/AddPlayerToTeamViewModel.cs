using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class AddPlayerToTeamViewModel
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }

        public List<Player> Players { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
    }
}
