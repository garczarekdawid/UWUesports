using UWUesports.Web.Models;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<ApplicationUser>> GetUsersAsync(string? searchTerm, int page, int pageSize);
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(UserCreateViewModel model);
        Task<(bool Success, IEnumerable<string> Errors)> UpdateUserAsync(UserEditViewModel model);
        Task<bool> DeleteUserAsync(string id);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<List<RoleCheckboxViewModel>> GetAllRolesForUserAsync(ApplicationUser user);
    }
}
