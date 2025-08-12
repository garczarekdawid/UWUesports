using UWUesports.Web.Models.Domain;
using UWUesports.Web.Repositories.Interfaces;
using UWUesports.Web.Data; // Twój DbContext

namespace UWUesports.Web.Repositories
{
    public class OrganizationRoleRepository : IOrganizationRoleRepository
    {

        private readonly UWUesportDbContext _context;

        public OrganizationRoleRepository(UWUesportDbContext context)
        {
            _context = context;
        }

        public void Create(OrganizationRole role)
        {
            _context.OrganizationRoles.Add(role);
        }

        public void Delete(int id)
        {
            var role = _context.OrganizationRoles.FirstOrDefault(r => r.Id == id);
            if (role != null)
            {
                _context.OrganizationRoles.Remove(role);
            }
        }

        public IEnumerable<OrganizationRole> GetAll()
        {
            return _context.OrganizationRoles.ToList();
        }

        public OrganizationRole GetById(int id)
        {
            return _context.OrganizationRoles.FirstOrDefault(r => r.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(OrganizationRole role)
        {
            _context.OrganizationRoles.Update(role);
        }
    }
}
