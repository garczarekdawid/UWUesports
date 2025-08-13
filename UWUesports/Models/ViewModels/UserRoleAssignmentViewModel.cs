using UWUesports.Web.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Models.Domain;

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
        public List<OrganizationRole> Roles { get; set; }

        [BindNever]
        public List<ApplicationUser> Users { get; set; }

        public UserRoleAssignmentViewModel()
        {
            Organizations = new List<Organization>();
            Roles = new List<OrganizationRole>();
            Users = new List<ApplicationUser>();
        }
    }
}