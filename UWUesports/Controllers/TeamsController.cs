using System;
using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace UWUesports.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly UWUesportDbContext _context;

        public TeamsController(UWUesportDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams
                .Include(t => t.TeamPlayers)
                    .ThenInclude(tp => tp.Player)
                .ToListAsync();

            return View(teams);
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
                return View(team); // Pokaże błędy, np. "Nickname is required"
            }

            var exists = await _context.Teams.AnyAsync(t => t.Name == team.Name);

            if (exists)
            {
                ModelState.AddModelError("Name", "Trużyna o takiej nazwie już istnieje.");
                return View(team); // Pokaże ten niestandardowy błąd
            }

            _context.Add(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
