using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Services;
using UWUesports.Web.Services.Interfaces;

namespace UWUesports.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserDashboardService _userDashboardService;

        public UserController(IUserDashboardService userDashboardService)
        {
            _userDashboardService = userDashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // Tutaj musisz podać userId do serwisu
            int userId = 1; // np. pobrane z ASP.Identity: User.GetUserId() po napisaniu metody rozszerzenia
            var model = await _userDashboardService.GetUserDashboardAsync(userId);
            return View(model);
        }
    }
}
