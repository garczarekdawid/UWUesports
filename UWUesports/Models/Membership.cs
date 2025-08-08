namespace UWUesports.Web.Models
{
    public class Membership
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int UserId  { get; set; }
        public User User { get; set; }
    }
}
