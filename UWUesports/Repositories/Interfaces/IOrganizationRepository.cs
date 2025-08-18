using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization> GetByIdAsync(int id);
        IQueryable<Organization> GetAllAsync(); // <- nowa metoda
        IQueryable<Organization> SearchByNameAsync(string name); // <- nowa metoda
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
