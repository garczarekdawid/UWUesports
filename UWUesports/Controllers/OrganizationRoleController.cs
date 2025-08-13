using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class OrganizationRoleController : Controller
    {
        private readonly IOrganizationRoleService _organizationRoleService;

        public OrganizationRoleController(IOrganizationRoleService organizationRoleService)
        {
            _organizationRoleService = organizationRoleService;
        }
  
        public async Task<IActionResult> Index(string searchName = "", int page = 1, int pageSize = 5)
        {
            int[] allowedPageSizes = new[] { 5, 10, 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
                pageSize = 5;

            var roles = await _organizationRoleService.GetAllPaginatedAsync(page, pageSize, searchName);

            ViewData["AllowedPageSizes"] = allowedPageSizes;
            ViewData["searchName"] = searchName;

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var organizations = await _organizationRoleService.GetAllOrganizationsAsync();

            var viewModel = new OrganizationRoleCreateViewModel
            {
                Organizations = organizations.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.Name
                })
            };

            return View(viewModel);
        }

        // POST: OrganizationRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationRoleCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // w razie błędu trzeba odtworzyć listę
                var organizations = await _organizationRoleService.GetAllOrganizationsAsync();
                viewModel.Organizations = organizations.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.Name
                });
                return View(viewModel);
            }

            var role = new OrganizationRole
            {
                Name = viewModel.Name,
                OrganizationId = viewModel.OrganizationId
            };

            await _organizationRoleService.AddAsync(role);
            return RedirectToAction(nameof(Index));
        }

        // GET: OrganizationRole/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return NotFound();

            var role = await _organizationRoleService.GetByIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }


        // POST: OrganizationRole/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _organizationRoleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Index));

            var role = await _organizationRoleService.GetByIdAsync(id);
            if (role == null) return RedirectToAction(nameof(Index));

            var organizations = await _organizationRoleService.GetAllOrganizationsAsync();
            var viewModel = new OrganizationRoleEditViewModel
            {
                Id = role.Id,
                Name = role.Name,
                OrganizationId = role.OrganizationId,
                Organizations = organizations.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.Name
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrganizationRoleEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var organizations = await _organizationRoleService.GetAllOrganizationsAsync();
                viewModel.Organizations = organizations.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.Name
                });
                return View(viewModel);
            }

            var role = new OrganizationRole
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                OrganizationId = viewModel.OrganizationId
            };

            await _organizationRoleService.UpdateAsync(role);
            return RedirectToAction(nameof(Index));
        }
    }
}
