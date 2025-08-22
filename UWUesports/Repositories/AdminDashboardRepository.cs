using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UWUesports.Web.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrganizationRepository _orgRepo;
        private readonly ITeamRepository _teamRepo;

        public AdminDashboardRepository(UserManager<ApplicationUser> userManager,
                                        IOrganizationRepository orgRepo,
                                        ITeamRepository teamRepo)
        {
            _userManager = userManager;
            _orgRepo = orgRepo;
            _teamRepo = teamRepo;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<int> GetTotalOrganizationsAsync()
        {
            return await _orgRepo.GetTotalOrganizationsAsync();
        }

        public async Task<int> GetTotalTeamsAsync()
        {
            return await _teamRepo.GetTotalTeamsAsync();
        }
    }
}
