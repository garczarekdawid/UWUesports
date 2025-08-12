using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class AssignGlobalRoleViewModel
    {
        public int? UserId { get; set; }
        public string RoleId { get; set; }  // IdentityRole ma Id jako string lub int (w zależności od konfiguracji)

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<IdentityRole<int>> Roles { get; set; } = new List<IdentityRole<int>>();
    }
}
