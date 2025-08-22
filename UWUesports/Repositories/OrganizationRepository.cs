using UWUesports.Web.Data;
using UWUesports.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly UWUesportDbContext _context;

        public OrganizationRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Organization> GetAllAsync()
        {
            return _context.Organizations.Include(o => o.Teams).AsQueryable();
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _context.Organizations.Include(o => o.Teams).FirstOrDefaultAsync(o => o.Id == id);
        }

        public IQueryable<Organization> SearchByNameAsync(string name)
        {
            return _context.Organizations
                .Include(o => o.Teams)
                .Where(o => o.Name.ToLower().Contains(name.ToLower()))
                .AsQueryable();
        }

        public async Task UpdateAsync(Organization organization)
        {
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Organizations.AnyAsync(o => o.Id == id);
        }

        public async Task<int> GetTotalOrganizationsAsync()
        {
            return await _context.Organizations.CountAsync();
        }

        public async Task<List<OrganizationRoleViewModel>> GetUserOrganizationsWithRolesAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                        .ThenInclude(t => t.Organization)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Role)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Organization)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new List<OrganizationRoleViewModel>();

            var organizationsFromTeams = user.TeamPlayers
                .Select(tp => tp.Team.Organization!)
                .Distinct()
                .ToList();

            var rolesInOrganizations = user.RoleAssignments
                .Select(ra => new OrganizationRoleViewModel
                {
                    OrganizationId = ra.OrganizationId,
                    OrganizationName = ra.Organization!.Name,
                    RoleName = ra.Role!.Name
                })
                .ToList();

            var result = organizationsFromTeams
                .Select(org => new OrganizationRoleViewModel
                {
                    OrganizationId = org.Id,
                    OrganizationName = org.Name,
                    RoleName = rolesInOrganizations
                        .Where(r => r.OrganizationId == org.Id)
                        .Select(r => r.RoleName)
                        .FirstOrDefault() ?? "No role"
                })
                .ToList();

            return result;
        }

    }
}

