using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nick jest wymagany.")]
        [MaxLength(50, ErrorMessage = "Nick może mieć maksymalnie 50 znaków.")]
        [MinLength(3, ErrorMessage = "Nick musi mieć minimalnie 3 znaki.")]
        public string Nickname { get; set; }

        public List<TeamPlayer> TeamPlayers { get; set; } = new();
    }
}
