using UWUesports.Web.Data;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Repositories;

namespace UWUesports.Web.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _repository;
        private readonly ITeamRepository _teamRepository;
        private readonly IOrganizationRoleRepository _roleRepository;

        public OrganizationService(IOrganizationRepository repository, ITeamRepository teamRepository, IOrganizationRoleRepository roleRepository)
        {
            _repository = repository;
            _teamRepository = teamRepository;
            _roleRepository = roleRepository;
        }

        public async Task CreateAsync(Organization organization)
        {
            await _repository.AddAsync(organization);
            // Automatyczne role dla organizacji
            // Teraz organization.Id ma prawidłową wartość
            var defaultRoles = new List<OrganizationRole>
            {
                new OrganizationRole { Name = "Trainer", OrganizationId = organization.Id },
                new OrganizationRole { Name = "Manager", OrganizationId = organization.Id },
                new OrganizationRole { Name = "Player", OrganizationId = organization.Id }
            };

            foreach (var role in defaultRoles)
            {
                await _roleRepository.AddAsync(role);
            }
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

            var availableTeams = await _teamRepository.GetAll()
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
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team != null)
            {
                team.OrganizationId = organizationId;
                await _teamRepository.UpdateAsync(team);
            }
        }

        public async Task RemoveTeamAsync(int organizationId, int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team != null && team.OrganizationId == organizationId)
            {
                team.OrganizationId = null;
                await _teamRepository.UpdateAsync(team);
            }
        }

        public async Task UpdateAsync(Organization organization)
        {
            await _repository.UpdateAsync(organization);
        }
    }
}

