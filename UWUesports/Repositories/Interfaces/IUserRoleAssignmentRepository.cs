using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Models;
using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IUserRoleAssignmentRepository
    {
        // Pobranie wszystkich jako IQueryable (do paginacji/filtrowania)
        IQueryable<UserRoleAssignment> GetAllAssignmentsQuery();

        // Pobranie pojedynczego przypisania
        Task<UserRoleAssignment> GetAssignmentAsync(int userId, int organizationId, int roleId);

        // Dodawanie/usuwanie z zapisem do bazy
        Task AddAsync(UserRoleAssignment assignment);
        Task RemoveAsync(UserRoleAssignment assignment);

        // Sprawdzenie istnienia
        Task<bool> ExistsAsync(int userId, int organizationId, int roleId);

        // Pobranie organizacji / ról / użytkowników
        List<Organization> GetOrganizations();
        List<IdentityRole<int>> GetRoles();
        List<OrganizationRole> GetRolesByOrganization(int organizationId);

        // Ogólny zapis zmian, jeśli potrzebny
        Task SaveChangesAsync();

        Task<List<ApplicationUser>> GetUsersByOrganizationAsync(int organizationId);
        Task<List<OrganizationRole>> GetRolesByOrganizationAsync(int organizationId);
    }
}
