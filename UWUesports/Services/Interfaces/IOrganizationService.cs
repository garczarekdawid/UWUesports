using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<PaginatedList<Organization>> GetPaginatedAsync(string searchName, int page, int pageSize);
        Task<Organization> GetByIdAsync(int id);
        Task CreateAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(int id);
        Task<OrganizationDetailsViewModel> GetDetailsAsync(int id);
        Task AssignTeamAsync(int organizationId, int teamId);
        Task RemoveTeamAsync(int organizationId, int teamId);

        Task<int> GetTotalOrganizationsAsync();
    }
}
