using Microsoft.AspNetCore.Mvc;
using SafeCasino.Services;

namespace SafeCasino.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILocalizationService _localizationService;
        private readonly IGameApiService _gameApiService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            ILocalizationService localizationService,
            IGameApiService gameApiService,
            ILogger<AdminController> logger)
        {
            _localizationService = localizationService;
            _gameApiService = gameApiService;
            _logger = logger;
        }

        private bool IsAdminLoggedIn()
        {
            var userType = HttpContext.Session.GetString("UserType");
            return userType == "Admin";
        }

        private IActionResult RedirectIfNotAdmin()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("AdminLogin", "Account");
            }
            return null;
        }

        public IActionResult Index()
        {
            var redirect = RedirectIfNotAdmin();
            if (redirect != null) return redirect;

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            return View();
        }

        public async Task<IActionResult> Games()
        {
            var redirect = RedirectIfNotAdmin();
            if (redirect != null) return redirect;

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            var filters = new Models.FilterOptions { PageSize = 100 };
            var response = await _gameApiService.GetGamesAsync(filters);

            return View(response);
        }

        public IActionResult Users()
        {
            var redirect = RedirectIfNotAdmin();
            if (redirect != null) return redirect;

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            return View();
        }

        public IActionResult Settings()
        {
            var redirect = RedirectIfNotAdmin();
            if (redirect != null) return redirect;

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            return View();
        }

        public IActionResult Reports()
        {
            var redirect = RedirectIfNotAdmin();
            if (redirect != null) return redirect;

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                _logger.LogInformation("Admin {Username} logged out", username);
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
