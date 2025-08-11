using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UWUesports.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        public string Nickname { get; set; }

        public List<UserRoleAssignment> RoleAssignments { get; set; } = new();

        public List<Membership> TeamPlayers { get; set; } = new();
    }
}
