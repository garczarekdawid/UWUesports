namespace UWUesports.Web.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public int CurrentOrganizationId { get; set; }
        public string CurrentOrganizationName { get; set; } = string.Empty;
        public List<string> CurrentUserRoles { get; set; } = new(); // z UserRoleAssignment
        public List<string> TeamsInCurrentOrganization { get; set; } = new(); // nazwy drużyn, w których jest user
    }
}

