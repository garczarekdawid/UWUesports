using UWUesports.Web.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace UWUesports.Web.Models.ViewModels
{
    public class UserRoleAssignmentViewModel
    {
        [Required(ErrorMessage = "Użytkownik jest wymagany")]
        [Display(Name = "Użytkownik")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Organizacja jest wymagana")]
        [Display(Name = "Organizacja")]
        public int? OrganizationId { get; set; }

        [Required(ErrorMessage = "Rola jest wymagana")]
        [Display(Name = "Rola")]
        public int? RoleId { get; set; }

        [BindNever]
        public List<Organization> Organizations { get; set; }

        [BindNever]
        public List<IdentityRole<int>> Roles { get; set; }

        [BindNever]
        public List<ApplicationUser> Users { get; set; }

        public UserRoleAssignmentViewModel()
        {
            Organizations = new List<Organization>();
            Roles = new List<IdentityRole<int>>();
            Users = new List<ApplicationUser>();
        }
    }
}