using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<PaginatedList<ApplicationUser>> GetUsersAsync(string? searchTerm, int page, int pageSize)
        {
            var usersQuery = _userManager.Users
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Role)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Organization)
                .Include(u => u.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                    .ThenInclude(t => t.Organization)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                usersQuery = usersQuery.Where(u =>
                    u.Nickname.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.RoleAssignments.Any(ra => ra.Role.Name.Contains(searchTerm)) ||
                    u.RoleAssignments.Any(ra => ra.Organization.Name.Contains(searchTerm)) ||
                    u.TeamPlayers.Any(tp => tp.Team.Name.Contains(searchTerm)) ||
                    u.TeamPlayers.Any(tp => tp.Team.Organization.Name.Contains(searchTerm))
                );
            }

            var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(
                usersQuery.AsNoTracking(), page, pageSize
            );

            foreach (var user in paginatedUsers.Items)
            {
                user.GlobalRoles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            return paginatedUsers;
        }

        public Task<ApplicationUser?> GetByIdAsync(string id) => _userManager.FindByIdAsync(id);

        public async Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(UserCreateViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return (false, new[] { "Użytkownik z takim emailem już istnieje." });

            var user = new ApplicationUser
            {
                Nickname = model.Nickname,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, "USER");

            return (true, Enumerable.Empty<string>());
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateUserAsync(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null) return (false, new[] { "Użytkownik nie istnieje." });

            user.Nickname = model.Nickname;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, updateResult.Errors.Select(e => e.Description));

            var allRoles = await _roleManager.Roles.ToListAsync();
            var selectedRoleNames = allRoles
                .Where(r => model.SelectedRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToList();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = selectedRoleNames.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(selectedRoleNames);

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded) return (false, addResult.Errors.Select(e => e.Description));
            }

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded) return (false, removeResult.Errors.Select(e => e.Description));
            }

            return (true, Enumerable.Empty<string>());
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public Task<List<string>> GetUserRolesAsync(ApplicationUser user) => _userManager.GetRolesAsync(user).ContinueWith(t => t.Result.ToList());

        public async Task<List<RoleCheckboxViewModel>> GetAllRolesForUserAsync(ApplicationUser user)
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            return allRoles.Select(r => new RoleCheckboxViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList();
        }
    }
}
