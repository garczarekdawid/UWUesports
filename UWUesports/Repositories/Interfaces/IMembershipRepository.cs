using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IMembershipRepository
    {
        Task<bool> ExistsAsync(int teamId, int userId);
        Task AddAsync(Membership membership);
        Task AddRangeAsync(IEnumerable<Membership> memberships);
        Task RemoveAsync(Membership membership);
        Task<Membership?> GetAsync(int teamId, int userId);
        Task<IEnumerable<int>> GetExistingUserIdsAsync(int teamId, IEnumerable<int> userIds);
        Task SaveChangesAsync();
    }
}
