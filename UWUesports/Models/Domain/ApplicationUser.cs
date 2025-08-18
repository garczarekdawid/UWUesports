using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UWUesports.Web.Models.Domain
{
    public class ApplicationUser : IdentityUser<int>
    {

        public string Nickname { get; set; }

        public List<UserRoleAssignment> RoleAssignments { get; set; } = new();

        public List<Membership> TeamPlayers { get; set; } = new();

        [NotMapped] // żeby EF nie próbował mapować na DB
        public IList<string> GlobalRoles { get; set; } = new List<string>();
    }
}
