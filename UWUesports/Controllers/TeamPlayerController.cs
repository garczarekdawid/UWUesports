using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class TeamPlayerController : Controller
    {
        private readonly UWUesportDbContext _context;

        public TeamPlayerController(UWUesportDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET
        public async Task<IActionResult> Create()
        {
            var model = new AddPlayerToTeamViewModel
            {
                Players = await _context.Players.ToListAsync(),
                Teams = await _context.Teams.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPlayerToTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // załaduj listy ponownie, jeśli trzeba
                model.Teams = await _context.Teams.ToListAsync();
                model.Players = await _context.Players.ToListAsync();
                return View(model);
            }

            // 🔍 sprawdzenie czy relacja już istnieje
            bool alreadyAssigned = await _context.TeamPlayers
                .AnyAsync(tp => tp.TeamId == model.TeamId && tp.PlayerId == model.PlayerId);

            if (alreadyAssigned)
            {
                ModelState.AddModelError("", "Ten gracz jest już przypisany do tej drużyny.");
                model.Teams = await _context.Teams.ToListAsync();
                model.Players = await _context.Players.ToListAsync();
                return View(model);
            }

            var teamPlayer = new TeamPlayer
            {
                TeamId = model.TeamId,
                PlayerId = model.PlayerId
            };

            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Teams");
        }
    }
}
