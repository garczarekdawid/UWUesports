using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class UserWithGlobalRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> GlobalRoles { get; set; }
    }
}
