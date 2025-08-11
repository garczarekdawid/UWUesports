using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.ViewModels
{
    public class RoleCreateViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
