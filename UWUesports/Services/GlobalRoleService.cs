using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Services
{
    public class GlobalRoleService : IGlobalRoleService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public GlobalRoleService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityRole<int>>> GetRolesAsync(string? searchName)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
                rolesQuery = rolesQuery.Where(r => r.Name.Contains(searchName));

            return await rolesQuery.AsNoTracking().ToListAsync();
        }

        public async Task<IdentityRole<int>?> GetRoleByIdAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });

            var role = new IdentityRole<int>(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(int id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            role.Name = roleName;
            role.NormalizedName = roleName.ToUpperInvariant();

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return IdentityResult.Success;

            return await _roleManager.DeleteAsync(role);
        }
    }
}
