using System.ComponentModel.DataAnnotations;
using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class OrganizationDetailsViewModel
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public List<Team> AvailableTeams { get; set; } = new();

        [Required(ErrorMessage = "Wybierz drużynę.")]
        public int? SelectedTeamId { get; set; } // do bindowania wyboru z formularza
    }
}
