using UWUesports.Web.Models;

namespace UWUesports.Web.Models.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public List<RoleCheckboxViewModel> AllRoles { get; set; } = new();
        public int[] SelectedRoleIds { get; set; } = new int[0];
    }
}
