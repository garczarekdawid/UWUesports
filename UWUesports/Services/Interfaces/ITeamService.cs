using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface ITeamService
    {
        //Task<List<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task AddTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(int id);
        Task<bool> TeamExistsByNameAsync(string name);
        Task<TeamDetailsViewModel?> GetTeamDetailsAsync(int id);
        Task<PaginatedList<Team>> GetPaginatedAsync(string searchName, int page, int pageSize);

        Task<int> GetTotalTeamsAsync();
    }
}
