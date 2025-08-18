namespace UWUesports.Web.Models.Domain
{
    public class Membership
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
