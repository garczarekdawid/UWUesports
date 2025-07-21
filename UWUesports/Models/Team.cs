using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace UWUesports.Web.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa teamu jest wymagana.")]
        [MaxLength(50, ErrorMessage = "Nazwa może mieć maksymalnie 50 znaków.")]
        [MinLength(3, ErrorMessage = "Nazwa musi mieć minimalnie 3 znaki.")]
        public string Name { get; set; }

        public List<TeamPlayer> TeamPlayers { get; set; } = new();
    }
}
