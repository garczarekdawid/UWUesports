using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class AssignUserRoleViewModel
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public List<Organization> Organizations { get; set; }
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }  // dodaj to!
    }
}
