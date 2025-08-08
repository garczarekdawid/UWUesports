namespace UWUesports.Web.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserRoleAssignment> UserRoles { get; set; } = new();
    }
}
