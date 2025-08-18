using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;
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
        public List<OrganizationRole> GetRolesByOrganization(int organizationId)
        {
            return _context.OrganizationRoles
                .Where(r => r.OrganizationId == organizationId)
                .ToList();
        }

        // Jeśli chcesz ogólny zapis (rzadziej potrzebny)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<ApplicationUser>> GetUsersByOrganizationAsync(int organizationId)
        {
            return await _context.TeamPlayers
                .Include(m => m.Team)
                .Where(m => m.Team.OrganizationId == organizationId)
                .Select(m => m.User)
                .Distinct()
                .ToListAsync(); // <- asynchroniczne pobranie danych
        }

        public async Task<List<OrganizationRole>> GetRolesByOrganizationAsync(int organizationId)
        {
            return await _context.OrganizationRoles
                .Where(r => r.OrganizationId == organizationId)
                .ToListAsync();
        }
    }
}
