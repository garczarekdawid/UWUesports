using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace UWUesports.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string? searchNickname, string? searchEmail, int page = 1, int pageSize = 10)
        {
            var usersQuery = _userManager.Users
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Role)
                .Include(u => u.RoleAssignments)
                    .ThenInclude(ra => ra.Organization)
                .Include(u => u.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                    .ThenInclude(t => t.Organization)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchNickname))
            {
                usersQuery = usersQuery.Where(u => u.Nickname.Contains(searchNickname));
                ViewData["searchNickname"] = searchNickname;
            }

            if (!string.IsNullOrWhiteSpace(searchEmail))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(searchEmail));
                ViewData["searchEmail"] = searchEmail;
            }

            var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(
                usersQuery.AsNoTracking(), page, pageSize
            );

            foreach (var user in paginatedUsers.Items)
            {
                user.GlobalRoles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            ViewData["AllowedPageSizes"] = new[] { 5, 10, 25, 50, 100 };

            return View(paginatedUsers);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Użytkownik z takim emailem już istnieje.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Nickname = model.Nickname,
                Email = model.Email,
                UserName = model.Email
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "USER");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user); // role użytkownika jako lista string

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                AllRoles = allRoles.Select(r => new RoleCheckboxViewModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    IsSelected = userRoles.Contains(r.Name) // ustaw true jeśli użytkownik ma tę rolę
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            // Aktualizuj podstawowe dane użytkownika
            user.Nickname = model.Nickname;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            // Pobierz wszystkie role z bazy
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Wyciągnij nazwy ról, które zostały zaznaczone w formularzu (na podstawie ID)
            var selectedRoleNames = allRoles
                .Where(r => model.SelectedRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToList();

            // Pobierz aktualne role użytkownika
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Oblicz które role trzeba dodać, a które usunąć
            var rolesToAdd = selectedRoleNames.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(selectedRoleNames);

            // Dodaj nowe role
            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            // Usuń niepotrzebne role
            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Users/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Obsłuż błąd usuwania jeśli chcesz
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
