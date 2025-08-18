using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task AddPlayerAsync(int teamId, int userId)
        {
            if (!await _membershipRepository.ExistsAsync(teamId, userId))
            {
                await _membershipRepository.AddAsync(new Membership { TeamId = teamId, UserId = userId });
                await _membershipRepository.SaveChangesAsync();
            }
        }

        public async Task<int> AddPlayersAsync(int teamId, IEnumerable<int> userIds)
        {
            var existing = await _membershipRepository.GetExistingUserIdsAsync(teamId, userIds);
            var newPlayers = userIds.Except(existing)
                .Select(uid => new Membership { TeamId = teamId, UserId = uid });

            await _membershipRepository.AddRangeAsync(newPlayers);
            await _membershipRepository.SaveChangesAsync();

            return newPlayers.Count();
        }

        public async Task RemovePlayerAsync(int teamId, int userId)
        {
            var entity = await _membershipRepository.GetAsync(teamId, userId);
            if (entity != null)
            {
                await _membershipRepository.RemoveAsync(entity);
                await _membershipRepository.SaveChangesAsync();
            }
        }
    }
}
