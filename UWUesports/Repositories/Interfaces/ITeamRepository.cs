using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        IQueryable<Team> GetAll();
        Task<Team?> GetByIdAsync(int id);
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteAsync(Team team);
        Task<bool> ExistsByNameAsync(string name);
        Task<int> GetTotalTeamsAsync();
    }
}
