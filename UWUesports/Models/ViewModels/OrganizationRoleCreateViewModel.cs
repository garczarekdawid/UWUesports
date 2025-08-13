using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UWUesports.Web.Models.ViewModels
{
    public class OrganizationRoleCreateViewModel
    {
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> Organizations { get; set; }
    }
}
