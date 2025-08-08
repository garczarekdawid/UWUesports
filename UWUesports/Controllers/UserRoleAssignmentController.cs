using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;

public class UserRoleAssignmentController : Controller
{
    private readonly UWUesportDbContext _context;

    public UserRoleAssignmentController(UWUesportDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var vm = new AssignUserRoleViewModel
        {
            Users = _context.Users.ToList(),
            Organizations = _context.Organizations.ToList(),
            Roles = _context.Roles.ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(AssignUserRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Users = _context.Users.ToList();
            model.Organizations = _context.Organizations.ToList();
            model.Roles = _context.Roles.ToList();
            return View(model);
        }

        var exists = _context.UserRoleAssignments.Any(x =>
            x.UserId == model.UserId &&
            x.OrganizationId == model.OrganizationId &&
            x.RoleId == model.RoleId);

        if (exists)
        {
            TempData["Error"] = "To przypisanie już istnieje.";
            return RedirectToAction("Create");
        }

        var assignment = new UserRoleAssignment
        {
            UserId = model.UserId,
            OrganizationId = model.OrganizationId,
            RoleId = model.RoleId
        };

        _context.UserRoleAssignments.Add(assignment);
        _context.SaveChanges();

        return RedirectToAction("Index"); // Zrobimy później
    }

    public IActionResult Index()
    {
        var assignments = _context.UserRoleAssignments
            .Include(x => x.User)
            .Include(x => x.Organization)
            .Include(x => x.Role)
            .ToList();

        return View(assignments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int userId, int organizationId, int roleId)
    {
        var assignment = _context.UserRoleAssignments
            .FirstOrDefault(x => x.UserId == userId && x.OrganizationId == organizationId && x.RoleId == roleId);

        if (assignment != null)
        {
            _context.UserRoleAssignments.Remove(assignment);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }


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
            Users = _context.Users.ToList(),
            Organizations = _context.Organizations.ToList(),
            Roles = _context.Roles.ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int originalUserId, int originalOrganizationId, int originalRoleId, AssignUserRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Users = _context.Users.ToList();
            model.Organizations = _context.Organizations.ToList();
            model.Roles = _context.Roles.ToList();
            return View(model);
        }

        var original = _context.UserRoleAssignments
            .FirstOrDefault(x =>
                x.UserId == originalUserId &&
                x.OrganizationId == originalOrganizationId &&
                x.RoleId == originalRoleId);

        if (original == null)
            return NotFound();

        // Sprawdź, czy nowe przypisanie już istnieje
        bool duplicate = _context.UserRoleAssignments.Any(x =>
            x.UserId == model.UserId &&
            x.OrganizationId == model.OrganizationId &&
            x.RoleId == model.RoleId &&
            (x.UserId != originalUserId || x.OrganizationId != originalOrganizationId || x.RoleId != originalRoleId));

        if (duplicate)
        {
            TempData["Error"] = "Takie przypisanie już istnieje.";
            return RedirectToAction("Edit", new { userId = originalUserId, organizationId = originalOrganizationId, roleId = originalRoleId });
        }

        // Usuń stare przypisanie i dodaj nowe
        _context.UserRoleAssignments.Remove(original);
        _context.UserRoleAssignments.Add(new UserRoleAssignment
        {
            UserId = model.UserId,
            OrganizationId = model.OrganizationId,
            RoleId = model.RoleId
        });

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersByOrganization(int organizationId)
    {
        var users = await _context.Teams
            .Where(t => t.OrganizationId == organizationId)
            .SelectMany(t => t.TeamPlayers)
            .Select(tp => tp.User)
            .Distinct()
            .Select(u => new {
                id = u.Id,
                nickname = u.Nickname
            })
            .ToListAsync();

        return Json(users);
    }

    [HttpGet]
    public async Task<IActionResult> SearchUsers(string term)
    {
        var users = await _context.Users
            .Where(u => u.Nickname.Contains(term) || u.Email.Contains(term))
            .Select(u => new {
                id = u.Id,
                nickname = u.Nickname,
                organizations = u.RoleAssignments.Select(ra => new {
                    id = ra.Organization.Id,
                    name = ra.Organization.Name
                }).ToList()
            })
            .ToListAsync();

        return Json(users);
    }
}
