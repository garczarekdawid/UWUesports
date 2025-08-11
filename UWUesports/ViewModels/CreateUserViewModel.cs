using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
