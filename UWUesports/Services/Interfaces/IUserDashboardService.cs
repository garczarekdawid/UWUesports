using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IUserDashboardService
    {
        Task<UserDashboardViewModel> GetUserDashboardAsync(int userId, int? organizationId = null);
    }
}
