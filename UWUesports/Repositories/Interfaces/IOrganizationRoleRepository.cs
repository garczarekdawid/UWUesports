using UWUesports.Web.Models.Domain;

namespace UWUesports.Web.Repositories.Interfaces
{
    public interface IOrganizationRoleRepository
    {
        IEnumerable<OrganizationRole> GetAll();
        OrganizationRole GetById(int id);
        void Create(OrganizationRole role);
        void Update(OrganizationRole role);
        void Delete(int id);
        void Save(); // zapis zmian w bazie
    }
}
