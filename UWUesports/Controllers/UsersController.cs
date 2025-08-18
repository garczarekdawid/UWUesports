using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var users = await _userService.GetUsersAsync(search, page, pageSize);
            ViewData["AllowedPageSizes"] = new[] { 5, 10, 25, 50, 100 };
            ViewData["search"] = search;

            return View(users);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (success, errors) = await _userService.CreateUserAsync(model);
            if (!success)
            {
                foreach (var error in errors) ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var user = await _userService.GetByIdAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                AllRoles = await _userService.GetAllRolesForUserAsync(user)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (success, errors) = await _userService.UpdateUserAsync(model);
            if (!success)
            {
                foreach (var error in errors) ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
