using System.Collections.Generic;
using System.Threading.Tasks;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IOrganizationRoleService
    {
        //Task<IEnumerable<OrganizationRole>> GetAllAsync();
        Task<PaginatedList<OrganizationRole>> GetAllPaginatedAsync(
            int pageNumber, int pageSize, string searchName = "", int? organizationId = null);
        Task<OrganizationRole> GetByIdAsync(int id);
        Task AddAsync(OrganizationRole role);
        Task UpdateAsync(OrganizationRole role);
        Task DeleteAsync(int id);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
    }
}
