using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;

using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Repositories
{
    public class OrganizationRoleRepository : IOrganizationRoleRepository
    {
        private readonly UWUesportDbContext _context;

        public OrganizationRoleRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public IQueryable<OrganizationRole> GetAll()
        {
            return _context.OrganizationRoles
                  .Include(r => r.Organization)
                  .AsQueryable();
        }

        public async Task<IEnumerable<OrganizationRole>> GetAllAsync()
        {
            return await _context.OrganizationRoles.ToListAsync();
        }

        public async Task<OrganizationRole> GetByIdAsync(int id)
        {
            return await _context.OrganizationRoles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(OrganizationRole role)
        {
            await _context.OrganizationRoles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrganizationRole role)
        {
            _context.OrganizationRoles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role != null)
            {
                _context.OrganizationRoles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrganizationRole>> GetAllByOrganizationAsync(int organizationId)
        {
            return await _context.OrganizationRoles
                                 .Where(r => r.OrganizationId == organizationId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }
    }
}
