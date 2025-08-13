using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class UserRoleAssignmentController : Controller
    {
        private readonly IUserRoleAssignmentService _userRoleAssignmentService;

        public UserRoleAssignmentController(IUserRoleAssignmentService userRoleAssignmentService)
        {
            _userRoleAssignmentService = userRoleAssignmentService;
        }

        public async Task<IActionResult> Index(string search = "", int page = 1, int pageSize = 5)
        {
            int[] allowedPageSizes = new[] { 5, 10, 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
                pageSize = 5;

            var assignments = await _userRoleAssignmentService.GetAllPaginatedAsync(page, pageSize, search);

            ViewData["AllowedPageSizes"] = allowedPageSizes;
            ViewData["searchName"] = search;

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? organizationId)
        {
            var vm = await _userRoleAssignmentService.PrepareCreateViewModelAsync(organizationId);
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _userRoleAssignmentService.PrepareCreateViewModelAsync(model.OrganizationId);
                return View(model);
            }

            var result = await _userRoleAssignmentService.CreateAssignmentAsync(model);
            if (!result.Success)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction(nameof(Create), new { organizationId = model.OrganizationId });
            }

            TempData["Success"] = "Przypisanie roli zostało dodane.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int userId, int organizationId, int roleId)
        {
            var vm = await _userRoleAssignmentService.PrepareEditViewModelAsync(userId, organizationId, roleId);
            if (vm == null) return NotFound();

            ViewBag.OriginalUserId = userId;
            ViewBag.OriginalOrganizationId = organizationId;
            ViewBag.OriginalRoleId = roleId;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int originalUserId, int originalOrganizationId, int originalRoleId, UserRoleAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _userRoleAssignmentService.PrepareEditViewModelAsync(originalUserId, originalOrganizationId, originalRoleId);
                return View(model);
            }

            var result = await _userRoleAssignmentService.EditAssignmentAsync(originalUserId, originalOrganizationId, originalRoleId, model);
            if (!result.Success)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction(nameof(Edit), new { userId = originalUserId, organizationId = originalOrganizationId, roleId = originalRoleId });
            }

            TempData["Success"] = "Przypisanie roli zostało zaktualizowane.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int organizationId, int roleId)
        {
            var result = await _userRoleAssignmentService.DeleteAssignmentAsync(userId, organizationId, roleId);
            if (!result.Success)
                TempData["Error"] = result.Error;
            else
                TempData["Success"] = "Przypisanie roli zostało usunięte.";

            return RedirectToAction(nameof(Index));
        }
    }
}
