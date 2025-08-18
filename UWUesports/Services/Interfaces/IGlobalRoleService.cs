using Microsoft.AspNetCore.Identity;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IGlobalRoleService
    {
        Task<IEnumerable<IdentityRole<int>>> GetRolesAsync(string? searchName);
        Task<IdentityRole<int>?> GetRoleByIdAsync(int id);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(int id, string roleName);
        Task<IdentityResult> DeleteRoleAsync(int id);
    }
}
