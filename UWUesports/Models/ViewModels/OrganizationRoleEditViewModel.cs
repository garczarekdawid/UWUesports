using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UWUesports.Web.Models.ViewModels
{
    public class OrganizationRoleEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int OrganizationId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Organizations { get; set; }
    }
}
