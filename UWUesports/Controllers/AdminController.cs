using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _adminService;

        public AdminController(IAdminDashboardService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _adminService.GetDashboardAsync();
            return View(model);
        }
    }
}
