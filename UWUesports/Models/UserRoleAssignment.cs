namespace UWUesports.Web.Models
{
    public class UserRoleAssignment
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
