using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Models;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Services
{
    public class OrganizationRoleService : IOrganizationRoleService
    {
        private readonly IOrganizationRoleRepository _roleRepository;

        public OrganizationRoleService(IOrganizationRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<PaginatedList<OrganizationRole>> GetAllPaginatedAsync(
            int pageNumber, int pageSize, string searchName = "", int? organizationId = null)
        {
            var query = _roleRepository.GetAll();

            // Jeśli podano organizationId, filtrujemy po nim
            if (organizationId.HasValue)
            {
                query = query.Where(r => r.OrganizationId == organizationId.Value);
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                var lowerSearch = searchName.ToLower();
                query = query.Where(r =>
                    r.Name.ToLower().Contains(lowerSearch) ||
                    (r.Organization != null && r.Organization.Name.ToLower().Contains(lowerSearch))
                );
            }
            return await PaginatedList<OrganizationRole>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<OrganizationRole> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(OrganizationRole role)
        {
            await _roleRepository.AddAsync(role);
        }

        public async Task UpdateAsync(OrganizationRole role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _roleRepository.GetAllOrganizationsAsync();
        }
    }
}
