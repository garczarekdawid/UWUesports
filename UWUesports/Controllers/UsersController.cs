using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UWUesportDbContext _context;

        public UsersController(UWUesportDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchEmail = "", string searchNickname = "", int page = 1, int pageSize = 5)
        {
            var query = _context.Users
                    .Include(u => u.TeamPlayers)
                        .ThenInclude(tp => tp.Team)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchEmail))
                query = query.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            if (!string.IsNullOrWhiteSpace(searchNickname))
                query = query.Where(u => u.Nickname.ToLower().Contains(searchNickname.ToLower()));

            var model = await PaginatedList<User>.CreateAsync(query, page, pageSize);

            ViewData["searchEmail"] = searchEmail;
            ViewData["searchNickname"] = searchNickname;

            return View(model);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Użytkownik z takim emailem już istnieje.");
                return View(user);
            }

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var user = await _context.Users.FindAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
                return View(user);

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Users/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
