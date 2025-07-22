using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nick jest wymagany.")]
        [MaxLength(50, ErrorMessage = "Nick może mieć maksymalnie 50 znaków.")]
        [MinLength(3, ErrorMessage = "Nick musi mieć minimalnie 3 znaki.")]
        public string Nickname { get; set; }

        public string Role { get; set; }  // np. User, Coach, Admin

        public List<TeamPlayer> TeamPlayers { get; set; } = new();
    }
}
