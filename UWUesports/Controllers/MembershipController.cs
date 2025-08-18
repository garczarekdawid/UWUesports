using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayer(int teamId, int playerId)
        {
            await _membershipService.AddPlayerAsync(teamId, playerId);
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlayers(int teamId, int[] playerIds)
        {
            if (playerIds == null || playerIds.Length == 0)
            {
                TempData["Error"] = "Musisz wybrać przynajmniej jednego gracza.";
                return RedirectToAction("Details", "Teams", new { id = teamId });
            }

            var added = await _membershipService.AddPlayersAsync(teamId, playerIds);
            TempData["Success"] = $"Dodano {added} graczy do drużyny.";
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(int teamId, int playerId)
        {
            await _membershipService.RemovePlayerAsync(teamId, playerId);
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }
    }
}
