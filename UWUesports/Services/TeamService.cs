using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamService(ITeamRepository teamRepository, UserManager<ApplicationUser> userManager)
        {
            _teamRepository = teamRepository;
            _userManager = userManager;
        }


        public Task<Team?> GetTeamByIdAsync(int id) => _teamRepository.GetByIdAsync(id);

        public Task AddTeamAsync(Team team) => _teamRepository.AddAsync(team);

        public Task UpdateTeamAsync(Team team) => _teamRepository.UpdateAsync(team);

        public async Task<PaginatedList<Team>> GetPaginatedAsync(string searchName, int page, int pageSize)
        {
            var query = _teamRepository.GetAll();

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(t => t.Name.ToLower().Contains(searchName.ToLower()));
            }

            return await PaginatedList<Team>.CreateAsync(query, page, pageSize);
        }


        public async Task DeleteTeamAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team != null)
                await _teamRepository.DeleteAsync(team);
        }

        public Task<bool> TeamExistsByNameAsync(string name) => _teamRepository.ExistsByNameAsync(name);

        public async Task<TeamDetailsViewModel?> GetTeamDetailsAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return null;

            var allUsers = _userManager.Users.ToList(); // wszyscy użytkownicy
            var playerIds = team.TeamPlayers.Select(tp => tp.UserId).ToList();
            var availablePlayers = allUsers.Where(u => !playerIds.Contains(u.Id)).ToList();

            return new TeamDetailsViewModel
            {
                Team = team,
                PlayersInTeam = team.TeamPlayers.Select(tp => tp.User).ToList(),
                AvailablePlayers = availablePlayers
            };
        }

        public async Task<int> GetTotalTeamsAsync()
        {
            return await _teamRepository.GetTotalTeamsAsync();
        }
    }
}
