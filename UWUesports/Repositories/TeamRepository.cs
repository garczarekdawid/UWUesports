using UWUesports.Web.Data;
using UWUesports.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models.Domain;


namespace UWUesports.Web.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly UWUesportDbContext _context;

        public TeamRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public IQueryable<Team> GetAll()
        {
            return _context.Teams
                .Include(t => t.TeamPlayers)
                .ThenInclude(tp => tp.User)
                .AsQueryable();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.TeamPlayers)
                    .ThenInclude(tp => tp.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Teams.AnyAsync(t => t.Name == name);
        }

        public async Task<int> GetTotalTeamsAsync()
        {
            return await _context.Teams.CountAsync();
        }
    }
}
