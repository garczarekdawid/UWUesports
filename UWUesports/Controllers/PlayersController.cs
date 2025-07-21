using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;

namespace UWUesports.Web.Controllers
{
    public class PlayersController : Controller
    {
        private readonly UWUesportDbContext _context;

        public PlayersController(UWUesportDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //var players = await _context.Players.ToListAsync();
            var players = await _context.Players
                .Include(p => p.TeamPlayers)
                    .ThenInclude(tp => tp.Team)
                .ToListAsync();

            return View(players);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {

            if (!ModelState.IsValid)
            {
                return View(player); // Pokaże błędy, np. "Nickname is required"
            }

            var exists = await _context.Players.AnyAsync(p => p.Nickname == player.Nickname);

            if (exists)
            {
                ModelState.AddModelError("Nickname", "Gracz o takim nicku już istnieje.");
                return View(player); // Pokaże ten niestandardowy błąd
            }

            _context.Add(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
