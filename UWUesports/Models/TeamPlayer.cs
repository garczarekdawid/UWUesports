namespace UWUesports.Web.Models
{
    public class TeamPlayer
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int UserId  { get; set; }
        public User User { get; set; }
    }
}
