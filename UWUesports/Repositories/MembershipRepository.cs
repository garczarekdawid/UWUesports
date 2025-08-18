using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Repositories.Interfaces;

namespace UWUesports.Web.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly UWUesportDbContext _context;

        public MembershipRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int teamId, int userId)
        {
            return await _context.Membership.AnyAsync(m => m.TeamId == teamId && m.UserId == userId);
        }

        public async Task AddAsync(Membership membership)
        {
            await _context.Membership.AddAsync(membership);
        }

        public async Task AddRangeAsync(IEnumerable<Membership> memberships)
        {
            await _context.Membership.AddRangeAsync(memberships);
        }

        public async Task RemoveAsync(Membership membership)
        {
            _context.Membership.Remove(membership);
        }

        public async Task<Membership?> GetAsync(int teamId, int userId)
        {
            return await _context.Membership.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
        }

        public async Task<IEnumerable<int>> GetExistingUserIdsAsync(int teamId, IEnumerable<int> userIds)
        {
            return await _context.Membership
                .Where(m => m.TeamId == teamId && userIds.Contains(m.UserId))
                .Select(m => m.UserId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
