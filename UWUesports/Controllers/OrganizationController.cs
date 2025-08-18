using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Data;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _service;

        public OrganizationController(IOrganizationService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string searchName = "", int page = 1, int pageSize = 5)
        {
            ViewData["AllowedPageSizes"] = new[] { 5, 10, 20, 50, 100 };
            ViewData["searchName"] = searchName;

            var model = await _service.GetPaginatedAsync(searchName, page, pageSize);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var organization = await _service.GetByIdAsync(id.Value);
            if (organization == null) return NotFound();

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Organization organization)
        {
            if (id != organization.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(organization);
                return RedirectToAction(nameof(Index));
            }

            return View(organization);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(organization);
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _service.GetDetailsAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeam(OrganizationDetailsViewModel model)
        {
            await _service.AssignTeamAsync(model.OrganizationId, model.SelectedTeamId);
            return RedirectToAction(nameof(Details), new { id = model.OrganizationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTeam(int teamId, int organizationId)
        {
            await _service.RemoveTeamAsync(organizationId, teamId);
            return RedirectToAction(nameof(Details), new { id = organizationId });
        }
    }
}
