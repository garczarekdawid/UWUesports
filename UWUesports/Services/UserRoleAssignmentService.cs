using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Services
{
    public class UserRoleAssignmentService : IUserRoleAssignmentService
    {
        private readonly IUserRoleAssignmentRepository _userRoleAssignmentRepository;

        public UserRoleAssignmentService(IUserRoleAssignmentRepository userRoleAssignmentRepository)
        {
            _userRoleAssignmentRepository = userRoleAssignmentRepository;
        }

        public async Task<IEnumerable<UserRoleAssignment>> GetAllAssignmentsAsync()
        {
            return await _userRoleAssignmentRepository.GetAllAssignmentsQuery().ToListAsync();
        }

        public async Task<UserRoleAssignmentViewModel> PrepareCreateViewModelAsync(int? organizationId)
        {
            var users = organizationId.HasValue && organizationId != 0
                ? await _userRoleAssignmentRepository.GetUsersByOrganizationAsync(organizationId.Value)
                : new List<ApplicationUser>();

            var roles = organizationId.HasValue && organizationId != 0
                ? await _userRoleAssignmentRepository.GetRolesByOrganizationAsync(organizationId.Value)
                : new List<OrganizationRole>();

            return new UserRoleAssignmentViewModel
            {
                Organizations = _userRoleAssignmentRepository.GetOrganizations(),
                Roles = roles,
                Users = users,
                OrganizationId = organizationId ?? 0
            };
        }

        public async Task<(bool Success, string Error)> CreateAssignmentAsync(UserRoleAssignmentViewModel model)
        {
            if (await _userRoleAssignmentRepository.ExistsAsync(model.UserId.Value, model.OrganizationId.Value, model.RoleId.Value))
            {
                return (false, "To przypisanie już istnieje.");
            }

            var assignment = new UserRoleAssignment
            {
                UserId = model.UserId.Value,
                OrganizationId = model.OrganizationId.Value,
                RoleId = model.RoleId.Value
            };

            await _userRoleAssignmentRepository.AddAsync(assignment);
            return (true, null);
        }

        public async Task<UserRoleAssignmentViewModel> PrepareEditViewModelAsync(int userId, int organizationId, int roleId)
        {
            var assignment = await _userRoleAssignmentRepository.GetAssignmentAsync(userId, organizationId, roleId);
            if (assignment == null) return null;

            var users = await _userRoleAssignmentRepository.GetUsersByOrganizationAsync(assignment.OrganizationId);
            var roles = await _userRoleAssignmentRepository.GetRolesByOrganizationAsync(assignment.OrganizationId);


            return new UserRoleAssignmentViewModel
            {
                UserId = assignment.UserId,
                OrganizationId = assignment.OrganizationId,
                RoleId = assignment.RoleId,
                Organizations = _userRoleAssignmentRepository.GetOrganizations(),
                Roles = roles,
                Users = users
            };
        }


        public async Task<(bool Success, string Error)> EditAssignmentAsync(
            int originalUserId, int originalOrganizationId, int originalRoleId,
            UserRoleAssignmentViewModel model)
        {
            var original = await _userRoleAssignmentRepository.GetAssignmentAsync(originalUserId, originalOrganizationId, originalRoleId);
            if (original == null)
                return (false, "Przypisanie nie istnieje.");

            if (await _userRoleAssignmentRepository.ExistsAsync(model.UserId.Value, model.OrganizationId.Value, model.RoleId.Value) &&
                (model.UserId != originalUserId || model.OrganizationId != originalOrganizationId || model.RoleId != originalRoleId))
            {
                return (false, "Takie przypisanie już istnieje.");
            }

            await _userRoleAssignmentRepository.RemoveAsync(original);

            await _userRoleAssignmentRepository.AddAsync(new UserRoleAssignment
            {
                UserId = model.UserId.Value,
                OrganizationId = model.OrganizationId.Value,
                RoleId = model.RoleId.Value
            });

            return (true, null);
        }

        public async Task<(bool Success, string Error)> DeleteAssignmentAsync(int userId, int organizationId, int roleId)
        {
            var assignment = await _userRoleAssignmentRepository.GetAssignmentAsync(userId, organizationId, roleId);
            if (assignment == null)
                return (false, "Przypisanie nie istnieje.");

            await _userRoleAssignmentRepository.RemoveAsync(assignment);
            return (true, null);
        }

        public async Task<PaginatedList<UserRoleAssignment>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search = "")
        {
            var query = _userRoleAssignmentRepository.GetAllAssignmentsQuery();

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(ura =>
                    ura.User.Nickname.ToLower().Contains(lowerSearch) ||
                    ura.Organization.Name.ToLower().Contains(lowerSearch) ||
                    ura.Role.Name.ToLower().Contains(lowerSearch));
            }

            query = query.OrderBy(ura => ura.User.Nickname);

            return await PaginatedList<UserRoleAssignment>.CreateAsync(query, pageNumber, pageSize);
        }

    }
}
