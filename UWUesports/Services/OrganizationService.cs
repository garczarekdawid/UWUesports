using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _repository;
        private readonly UWUesportDbContext _context; // dla operacji na Teams

        public OrganizationService(IOrganizationRepository repository, UWUesportDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task CreateAsync(Organization organization)
        {
            await _repository.AddAsync(organization);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PaginatedList<Organization>> GetPaginatedAsync(string searchName, int page, int pageSize)
        {
            var query = string.IsNullOrEmpty(searchName)
                ? ( _repository.GetAllAsync()).AsQueryable()
                : ( _repository.SearchByNameAsync(searchName)).AsQueryable();

            return await PaginatedList<Organization>.CreateAsync(query, page, pageSize);
        }

        public async Task<OrganizationDetailsViewModel> GetDetailsAsync(int id)
        {
            var organization = await _repository.GetByIdAsync(id);
            if (organization == null) return null;

            var availableTeams = await _context.Teams
                .Where(t => t.OrganizationId == null)
                .ToListAsync();

            return new OrganizationDetailsViewModel
            {
                Organization = organization,
                OrganizationId = organization.Id,
                AvailableTeams = availableTeams
            };
        }

        public async Task AssignTeamAsync(int organizationId, int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team != null)
            {
                team.OrganizationId = organizationId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTeamAsync(int organizationId, int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team != null && team.OrganizationId == organizationId)
            {
                team.OrganizationId = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Organization organization)
        {
            await _repository.UpdateAsync(organization);
        }
    }
}

