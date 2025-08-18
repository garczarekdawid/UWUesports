using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayer(int teamId, int playerId)
        {
            if (!await _context.TeamPlayers.AnyAsync(tp => tp.TeamId == teamId && tp.UserId  == playerId))
            {
                _context.TeamPlayers.Add(new Membership { TeamId = teamId, UserId  = playerId });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }

        public async Task<IActionResult> AddPlayers(int teamId, int[] playerIds)
        {
            if (playerIds == null || playerIds.Length == 0)
            {
                TempData["Error"] = "Musisz wybrać przynajmniej jednego gracza.";
                return RedirectToAction("Details", "Teams", new { id = teamId });
            }

            var existingPlayerIds = await _context.TeamPlayers
                .Where(tp => tp.TeamId == teamId && playerIds.Contains(tp.UserId ))
                .Select(tp => tp.UserId )
                .ToListAsync();

            var newPlayers = playerIds.Except(existingPlayerIds)
                .Select(pid => new Membership { TeamId = teamId, UserId  = pid });

            _context.TeamPlayers.AddRange(newPlayers);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Dodano {newPlayers.Count()} graczy do drużyny.";
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(int teamId, int playerId)
        {
            var entity = await _context.TeamPlayers
                .FirstOrDefaultAsync(tp => tp.TeamId == teamId && tp.UserId  == playerId);

            if (entity != null)
            {
                _context.TeamPlayers.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Teams", new { id = teamId });
        }
    }
}
