using System.Collections.Generic;
using System.Threading.Tasks;
using UWUesports.Web.Models.Domain;


namespace UWUesports.Web.Services.Interfaces
{
    public interface IOrganizationRoleRepository
    {
        Task<IEnumerable<OrganizationRole>> GetAllAsync();
        Task<OrganizationRole> GetByIdAsync(int id);
        Task AddAsync(OrganizationRole role);
        Task UpdateAsync(OrganizationRole role);
        Task DeleteAsync(int id);
        IQueryable<OrganizationRole> GetAll(); // IQueryable do paginacji
        Task<IEnumerable<OrganizationRole>> GetAllByOrganizationAsync(int organizationId);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
    }
}
