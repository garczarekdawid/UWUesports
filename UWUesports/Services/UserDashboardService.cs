using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.Repositories.Interfaces;

namespace UWUesports.Web.Services
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IUserDashboardRepository _repo;

        public UserDashboardService(IUserDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserDashboardViewModel> GetUserDashboardAsync(int userId, int? organizationId = null)
        {
            var user = await _repo.GetUserWithTeamsAndRolesAsync(userId);
            if (user == null) return new UserDashboardViewModel();

            var organizationsFromTeams = user.TeamPlayers
                .Select(tp => tp.Team.Organization!)
                .Distinct()
                .ToList();

            var currentOrg = organizationId.HasValue
                ? organizationsFromTeams.FirstOrDefault(o => o.Id == organizationId.Value)
                : organizationsFromTeams.FirstOrDefault();

            if (currentOrg == null) return new UserDashboardViewModel();

            var rolesInCurrentOrg = user.RoleAssignments
                .Where(ra => ra.OrganizationId == currentOrg.Id)
                .Select(ra => ra.Role!.Name)
                .ToList();

            var teamsInCurrentOrg = user.TeamPlayers
                .Where(tp => tp.Team.OrganizationId == currentOrg.Id)
                .Select(tp => tp.Team.Name)
                .ToList();

            return new UserDashboardViewModel
            {
                CurrentOrganizationId = currentOrg.Id,
                CurrentOrganizationName = currentOrg.Name,
                CurrentUserRoles = rolesInCurrentOrg.Any() ? rolesInCurrentOrg : new List<string> { "No role" },
                TeamsInCurrentOrganization = teamsInCurrentOrg
            };
        }
    }
}
