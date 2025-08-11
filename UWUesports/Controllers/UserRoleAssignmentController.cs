using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class UserRoleAssignmentController : Controller
    {
        private readonly UWUesportDbContext _context;

        public UserRoleAssignmentController(UWUesportDbContext context)
        {
            _context = context;
        }

        // GET: UserRoleAssignment
        public IActionResult Index()
        {
            var assignments = _context.UserRoleAssignments
                .Include(x => x.User)
                .Include(x => x.Organization)
                .Include(x => x.Role)
                .ToList();

            return View(assignments);
        }

        // GET: UserRoleAssignment/Create
        [HttpGet]
        public IActionResult Create(int? organizationId)
        {
            var vm = new AssignUserRoleViewModel
            {
                Organizations = _context.Organizations.ToList(),
                Roles = _context.Roles.ToList(),
                Users = organizationId.HasValue && organizationId != 0
                    ? _context.Teams
                        .Where(t => t.OrganizationId == organizationId.Value)
                        .SelectMany(t => t.TeamPlayers)
                        .Select(tp => tp.User)
                        .Distinct()
                        .ToList()
                    : new List<User>(),
                OrganizationId = organizationId ?? 0
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AssignUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // jeśli błędy, zwróć ponownie widok z wczytanymi listami
                model.Organizations = _context.Organizations.ToList();
                model.Roles = _context.Roles.ToList();
                model.Users = model.OrganizationId != 0
                    ? _context.Teams
                        .Where(t => t.OrganizationId == model.OrganizationId)
                        .SelectMany(t => t.TeamPlayers)
                        .Select(tp => tp.User)
                        .Distinct()
                        .ToList()
                    : new List<User>();

                return View(model);
            }

            bool exists = _context.UserRoleAssignments.Any(x =>
                x.UserId == model.UserId &&
                x.OrganizationId == model.OrganizationId &&
                x.RoleId == model.RoleId);

            if (exists)
            {
                TempData["Error"] = "To przypisanie już istnieje.";
                return RedirectToAction(nameof(Create), new { organizationId = model.OrganizationId });
            }

            var assignment = new UserRoleAssignment
            {
                UserId = model.UserId.Value,
                OrganizationId = model.OrganizationId.Value,
                RoleId = model.RoleId.Value
            };

            _context.UserRoleAssignments.Add(assignment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        
        private IActionResult LoadUsersView(AssignUserRoleViewModel model)
        {
            model.Organizations = _context.Organizations.ToList();
            model.Roles = _context.Roles.ToList();

            if (model.OrganizationId.HasValue)
            {
                model.Users = _context.Teams
                    .Where(t => t.OrganizationId == model.OrganizationId.Value)
                    .SelectMany(t => t.TeamPlayers)
                    .Select(tp => tp.User)
                    .Distinct()
                    .ToList();
            }
            else
            {
                model.Users = new List<User>();
            }

            return View(model);
        }


        // GET: UserRoleAssignment/Edit
        [HttpGet]
        public IActionResult Edit(int userId, int organizationId, int roleId)
        {
            var assignment = _context.UserRoleAssignments
                .Include(x => x.User)
                .Include(x => x.Organization)
                .Include(x => x.Role)
                .FirstOrDefault(x => x.UserId == userId && x.OrganizationId == organizationId && x.RoleId == roleId);

            if (assignment == null)
                return NotFound();

            var vm = new AssignUserRoleViewModel
            {
                UserId = assignment.UserId,
                OrganizationId = assignment.OrganizationId,
                RoleId = assignment.RoleId,
                Organizations = _context.Organizations.ToList(),
                Roles = _context.Roles.ToList(),
                Users = _context.Teams
                    .Where(t => t.OrganizationId == assignment.OrganizationId)
                    .SelectMany(t => t.TeamPlayers)
                    .Select(tp => tp.User)
                    .Distinct()
                    .ToList()
            };

            ViewBag.OriginalUserId = assignment.UserId;
            ViewBag.OriginalOrganizationId = assignment.OrganizationId;
            ViewBag.OriginalRoleId = assignment.RoleId;

            return View(vm);
        }

        // POST: UserRoleAssignment/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int originalUserId, int originalOrganizationId, int originalRoleId, AssignUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Organizations = _context.Organizations.ToList();
                model.Roles = _context.Roles.ToList();
                model.Users = model.OrganizationId != 0
                    ? _context.Teams
                        .Where(t => t.OrganizationId == model.OrganizationId)
                        .SelectMany(t => t.TeamPlayers)
                        .Select(tp => tp.User)
                        .Distinct()
                        .ToList()
                    : new List<User>();

                return View(model);
            }

            try
            {
                var original = _context.UserRoleAssignments
                    .FirstOrDefault(x =>
                        x.UserId == originalUserId &&
                        x.OrganizationId == originalOrganizationId &&
                        x.RoleId == originalRoleId);

                if (original == null)
                    return NotFound();

                bool duplicate = _context.UserRoleAssignments.Any(x =>
                    x.UserId == model.UserId &&
                    x.OrganizationId == model.OrganizationId &&
                    x.RoleId == model.RoleId &&
                    (x.UserId != originalUserId || x.OrganizationId != originalOrganizationId || x.RoleId != originalRoleId));

                if (duplicate)
                {
                    TempData["Error"] = "Takie przypisanie już istnieje.";
                    return RedirectToAction(nameof(Edit), new { userId = originalUserId, organizationId = originalOrganizationId, roleId = originalRoleId });
                }

                _context.UserRoleAssignments.Remove(original);
                _context.UserRoleAssignments.Add(new UserRoleAssignment
                {
                    UserId = model.UserId.Value,
                    OrganizationId = model.OrganizationId.Value,
                    RoleId = model.RoleId.Value
                });

                _context.SaveChanges();

                TempData["Success"] = "Przypisanie roli zostało zaktualizowane.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Wystąpił błąd: {ex.Message}";
                return RedirectToAction(nameof(Edit), new { userId = originalUserId, organizationId = originalOrganizationId, roleId = originalRoleId });
            }
        }

        // POST: UserRoleAssignment/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int userId, int organizationId, int roleId)
        {
            var assignment = _context.UserRoleAssignments
                .FirstOrDefault(x => x.UserId == userId && x.OrganizationId == organizationId && x.RoleId == roleId);

            if (assignment != null)
            {
                try
                {
                    _context.UserRoleAssignments.Remove(assignment);
                    _context.SaveChanges();
                    TempData["Success"] = "Przypisanie roli zostało usunięte.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Wystąpił błąd: {ex.Message}";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}