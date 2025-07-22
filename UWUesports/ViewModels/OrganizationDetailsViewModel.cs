using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class OrganizationDetailsViewModel
    {
        public Organization Organization { get; set; }
        public List<Team> AvailableTeams { get; set; } = new();
        public int SelectedTeamId { get; set; } // do bindowania wyboru z formularza
    }
}
