using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;

using UWUesports.Web.Models;
using UWUesports.Web.Repositories.Interfaces;

namespace UWUesports.Web.Repositories
{
    public class UserRoleAssignmentRepository : IUserRoleAssignmentRepository
    {
        private readonly UWUesportDbContext _context;

        public UserRoleAssignmentRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        // Pobieranie wszystkich
        public IQueryable<UserRoleAssignment> GetAllAssignmentsQuery()
        {
            return _context.UserRoleAssignments
                .Include(x => x.User)
                .Include(x => x.Organization)
                .Include(x => x.Role);
        }

        // Pobranie pojedynczego przypisania
        public async Task<UserRoleAssignment> GetAssignmentAsync(int userId, int organizationId, int roleId)
        {
            return await _context.UserRoleAssignments
                .Include(x => x.User)
                .Include(x => x.Organization)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId && x.RoleId == roleId);
        }

        // Dodawanie
        public async Task AddAsync(UserRoleAssignment assignment)
        {
            await _context.UserRoleAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        // Usuwanie
        public async Task RemoveAsync(UserRoleAssignment assignment)
        {
            _context.UserRoleAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }

        // Sprawdzenie istnienia
        public async Task<bool> ExistsAsync(int userId, int organizationId, int roleId)
        {
            return await _context.UserRoleAssignments.AnyAsync(x => x.UserId == userId && x.OrganizationId == organizationId && x.RoleId == roleId);
        }

        // Pobranie organizacji / ról / użytkowników (synchron)
        public List<Organization> GetOrganizations() => _context.Organizations.ToList();
        public List<IdentityRole<int>> GetRoles() => _context.Roles.ToList();
        public List<ApplicationUser> GetUsersByOrganization(int organizationId)
        {
            return _context.Teams
                .Where(t => t.OrganizationId == organizationId)
                .SelectMany(t => t.TeamPlayers)
                .Select(tp => tp.User)
                .Distinct()
                .ToList();
        }

        // Jeśli chcesz ogólny zapis (rzadziej potrzebny)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
