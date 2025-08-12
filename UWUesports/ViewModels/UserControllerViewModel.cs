using Microsoft.AspNetCore.Identity;

namespace UWUesports.Web.ViewModels
{
    public class UserControllerViewModel
    {
    }

    public class UserListViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Teams { get; set; } = new List<string>();
        public List<string> Organizations { get; set; } = new List<string>();
        public List<string> GlobalRoles { get; set; } = new List<string>();
        public List<string> OrganizationRoles { get; set; } = new List<string>();
    }

    public class UserEditViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RoleId { get; set; } // global role selected

        public List<IdentityRole<int>> AvailableRoles { get; set; } = new List<IdentityRole<int>>();
    }
}
