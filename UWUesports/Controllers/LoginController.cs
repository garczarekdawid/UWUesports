using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Models.ViewModels;

namespace UWUesports.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Szukaj użytkownika po email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Nie znaleziono konta o podanym adresie e-mail.");
                return View(model);
            }

            // Użyj Email jako UserName - KLUCZOWA ZMIANA
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, // Używamy email jako nazwy użytkownika
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true); // Zmień na true dla lepszego debugowania

            if (result.Succeeded)
            {

                // Przekierowanie na podstawie roli
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Twoje konto zostało tymczasowo zablokowane.");
                return View(model);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("TwoFactor", new { returnUrl, model.RememberMe });
            }

            ModelState.AddModelError(nameof(model.Password), "Niepoprawne hasło.");
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
