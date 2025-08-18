using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class GlobalRolesController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public GlobalRolesController(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: GlobalRoles
        public async Task<IActionResult> Index(string? searchName, int page = 1, int pageSize = 10)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                rolesQuery = rolesQuery.Where(r => r.Name.Contains(searchName));
                ViewData["searchName"] = searchName;
            }

            var paginatedRoles = await PaginatedList<IdentityRole<int>>.CreateAsync(
                rolesQuery.AsNoTracking(),
                page,
                pageSize
            );

            ViewData["AllowedPageSizes"] = new[] { 5, 10, 25, 50, 100 };
            return View(paginatedRoles);
        }

        // GET: GlobalRoles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            return View(role);
        }

        // GET: GlobalRoles/Create
        public IActionResult Create() => View();

        // POST: GlobalRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GlobalRoleCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError("", "Role already exists.");
                return View(model);
            }

            var role = new IdentityRole<int>(model.RoleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // GET: GlobalRoles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            return View(role);
        }

        // POST: GlobalRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();

            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View(role);
            }

            role.Name = roleName;
            role.NormalizedName = roleName.ToUpperInvariant();
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(role);
        }

        // POST: GlobalRoles/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
                await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
    }
}
