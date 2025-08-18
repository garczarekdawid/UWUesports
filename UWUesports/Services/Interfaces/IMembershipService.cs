namespace UWUesports.Web.Services.Interfaces
{
    public interface IMembershipService
    {
        Task AddPlayerAsync(int teamId, int userId);
        Task<int> AddPlayersAsync(int teamId, IEnumerable<int> userIds);
        Task RemovePlayerAsync(int teamId, int userId);
    }
}
