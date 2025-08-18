using System;
using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Data;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Models.ViewModels;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.Models.Domain;


namespace UWUesports.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<IActionResult> Index(string searchName = "", int page = 1, int pageSize = 5)
        {
            var model = await _teamService.GetPaginatedAsync(searchName, page, pageSize);

            ViewData["AllowedPageSizes"] = new[] { 5, 10, 20, 50, 100 };
            ViewData["searchName"] = searchName;

            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid) return View(team);

            if (await _teamService.TeamExistsByNameAsync(team.Name))
            {
                ModelState.AddModelError("Name", "Drużyna o takiej nazwie już istnieje.");
                return View(team);
            }

            await _teamService.AddTeamAsync(team);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null) return RedirectToAction(nameof(Index));
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id) return RedirectToAction(nameof(Index));
            if (!ModelState.IsValid) return View(team);

            await _teamService.UpdateTeamAsync(team);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teamService.DeleteTeamAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _teamService.GetTeamDetailsAsync(id);
            if (model == null) return RedirectToAction(nameof(Index));
            return View(model);
        }
    }
}