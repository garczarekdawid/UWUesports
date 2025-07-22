using System.ComponentModel.DataAnnotations;

namespace UWUesports.Web.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa organizacji jest wymagana.")]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<Team> Teams { get; set; } = new();

    }
}
