using UWUesports.Web.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UWUesports.Web.ViewModels
{
    public class AssignUserRoleViewModel
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
        public List<Role> Roles { get; set; }

        [BindNever]
        public List<User> Users { get; set; }

        public AssignUserRoleViewModel()
        {
            Organizations = new List<Organization>();
            Roles = new List<Role>();
            Users = new List<User>();
        }
    }
}