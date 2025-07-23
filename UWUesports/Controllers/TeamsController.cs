using System;
using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly UWUesportDbContext _context;

        public TeamsController(UWUesportDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index(string searchName = "", int page = 1, int pageSize = 5)
        {
            int[] allowedPageSizes = new[] { 5, 10, 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
                pageSize = 5;

            var query = _context.Teams
                .Include(t => t.TeamPlayers)
                    .ThenInclude(tp => tp.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(t => t.Name.ToLower().Contains(searchName.ToLower()));

            var model = await PaginatedList<Team>.CreateAsync(query, page, pageSize);

            ViewData["AllowedPageSizes"] = allowedPageSizes;
            ViewData["searchName"] = searchName;

            return View(model);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var exists = await _context.Teams.AnyAsync(t => t.Name == team.Name);
            if (exists)
            {
                ModelState.AddModelError("Name", "Drużyna o takiej nazwie już istnieje.");
                return View(team);
            }

            _context.Add(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var team = await _context.Teams.FindAsync(id);
            if (team == null) return RedirectToAction(nameof(Index));

            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id) return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                return View(team);
            }

            _context.Update(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return RedirectToAction(nameof(Index));

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return RedirectToAction(nameof(Index));

            var playersInTeam = await _context.TeamPlayers
                .Where(tp => tp.TeamId == id)
                .Select(tp => tp.User)
                .ToListAsync();

            var playerIdsInTeam = playersInTeam.Select(p => p.Id).ToList();

            var availablePlayers = await _context.Users
                .Where(p => !playerIdsInTeam.Contains(p.Id))
                .ToListAsync();

            var model = new TeamDetailsViewModel
            {
                Team = team,
                PlayersInTeam = playersInTeam,
                AvailablePlayers = availablePlayers
            };

            return View(model);
        }
    }
}