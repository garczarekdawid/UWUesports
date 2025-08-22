using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IUserDashboardRepository
    {
        Task<ApplicationUser?> GetUserWithTeamsAndRolesAsync(int userId);
    }
}
