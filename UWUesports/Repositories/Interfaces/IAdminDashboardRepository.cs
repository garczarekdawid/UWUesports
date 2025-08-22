namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalOrganizationsAsync();
        Task<int> GetTotalTeamsAsync();
    }
}
