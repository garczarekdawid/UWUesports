using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Repositories.Interfaces;

namespace UWUesports.Web.Repositories
{
    public class UserDashboardRepository : IUserDashboardRepository
    {
        private readonly UWUesportDbContext _context;

        public UserDashboardRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetUserWithTeamsAndRolesAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                        .ThenInclude(t => t.Organization)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Role)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Organization)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
