using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.Models.ViewModels
{
    public class GlobalRoleCreateViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
