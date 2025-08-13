using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Models
{
    public class UserRoleAssignment
    {
        public int UserId { get; set; }
        public ApplicationUser  User { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public int RoleId { get; set; }
        public OrganizationRole Role { get; set; }
    }
}
