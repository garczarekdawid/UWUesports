using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repo;

        public AdminDashboardService(IAdminDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            return new AdminDashboardViewModel
            {
                TotalUsers = await _repo.GetTotalUsersAsync(),
                TotalOrganizations = await _repo.GetTotalOrganizationsAsync(),
                TotalTeams = await _repo.GetTotalTeamsAsync()
            };
        }
    }
}
