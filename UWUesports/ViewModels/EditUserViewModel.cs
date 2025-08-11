using UWUesports.Web.Models;

namespace UWUesports.Web.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public List<RoleCheckboxViewModel> AllRoles { get; set; } = new();
        public int[] SelectedRoleIds { get; set; } = new int[0];
    }
}
