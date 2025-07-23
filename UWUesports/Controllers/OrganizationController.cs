using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UWUesports.Web.Data;
using UWUesports.Web.Models;
using UWUesports.Web.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly UWUesportDbContext _context;

        public OrganizationController(UWUesportDbContext context)
        {
            _context = context;
        }

        // GET: Organization - lista organizacji
        public async Task<IActionResult> Index(string searchName = "", int page = 1, int pageSize = 5)
        {
            int[] allowedPageSizes = new[] { 5, 10, 20, 50, 100 };
            if (!allowedPageSizes.Contains(pageSize))
                pageSize = 5;

            var query = _context.Organizations
                .Include(o => o.Teams)  // załaduj drużyny przypisane do organizacji
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(o => o.Name.ToLower().Contains(searchName.ToLower()));

            ViewData["AllowedPageSizes"] = allowedPageSizes;
            ViewData["searchName"] = searchName;

            var model = await PaginatedList<Organization>.CreateAsync(query, page, pageSize);

            return View(model);
        }



        // GET: Organization/Edit/5 - edycja organizacji
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
                return NotFound();

            return View(organization);
        }

        // POST: Organization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Organization organization)
        {
            if (id != organization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Organizations.Any(o => o.Id == organization.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(organization);
        }

        // GET: Organization/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // POST: Organization/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Organization/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var organization = await _context.Organizations
                .Include(o => o.Teams)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
                return NotFound();

            var availableTeams = await _context.Teams
                .Where(t => t.OrganizationId == null)
                .ToListAsync();

            var viewModel = new OrganizationDetailsViewModel
            {
                Organization = organization,
                OrganizationId = organization.Id, // ✅ tu ustawiamy wartość
                AvailableTeams = availableTeams
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeam(OrganizationDetailsViewModel model)
        {
            /*if (!ModelState.IsValid)
            {
                // Ponownie załaduj dane do widoku
                model.Organization = await _context.Organizations
                    .Include(o => o.Teams)
                    .FirstOrDefaultAsync(o => o.Id == model.OrganizationId);

                model.AvailableTeams = await _context.Teams
                    .Where(t => t.OrganizationId == null)
                    .ToListAsync();

                return View("Details", model);
            }*/

            var organization = await _context.Organizations
                .Include(o => o.Teams)
                .FirstOrDefaultAsync(o => o.Id == model.OrganizationId);

            if (organization == null)
                return NotFound();

            var team = await _context.Teams.FindAsync(model.SelectedTeamId);
            if (team == null)
                return NotFound();

            team.OrganizationId = organization.Id;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = model.OrganizationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTeam(int teamId, int organizationId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null || team.OrganizationId != organizationId)
            {
                return NotFound();
            }

            team.OrganizationId = null;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = organizationId });
        }

    }
}
