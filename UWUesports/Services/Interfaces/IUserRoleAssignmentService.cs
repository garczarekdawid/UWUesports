using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Services.Interfaces
{
    public interface IUserRoleAssignmentService
    {
        Task<IEnumerable<UserRoleAssignment>> GetAllAssignmentsAsync();
        Task<(bool Success, string Error)> CreateAssignmentAsync(UserRoleAssignmentViewModel model);
        Task<UserRoleAssignmentViewModel> PrepareEditViewModelAsync(int userId, int organizationId, int roleId);
        Task<(bool Success, string Error)> EditAssignmentAsync(int originalUserId, int originalOrganizationId, int originalRoleId, UserRoleAssignmentViewModel model);
        Task<(bool Success, string Error)> DeleteAssignmentAsync(int userId, int organizationId, int roleId);
        Task<PaginatedList<UserRoleAssignment>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search = "");
        Task<UserRoleAssignmentViewModel> PrepareCreateViewModelAsync(int? organizationId);
    }
}
