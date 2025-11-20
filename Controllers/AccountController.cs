using Microsoft.AspNetCore.Mvc;
using SafeCasino.Services;

namespace SafeCasino.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthenticationService authService,
            ILocalizationService localizationService,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _localizationService = localizationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            // If already logged in, redirect to home
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string returnUrl = "/")
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["ErrorMessage"] = "Voer alstublieft beide velden in.";
                return View();
            }

            var (success, userType) = _authService.ValidateCredentials(username, password);

            if (success)
            {
                // Store user info in session
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("UserType", userType);
                HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                _logger.LogInformation("User {Username} logged in as {UserType}", username, userType);

                // Redirect based on user type
                if (userType == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                // Redirect to return URL if it's local
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = "Ongeldige gebruikersnaam of wachtwoord.";
            return View();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            // If already logged in as admin, redirect
            var userType = HttpContext.Session.GetString("UserType");
            if (userType == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string username, string password)
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["ErrorMessage"] = "Voer alstublieft beide velden in.";
                return View();
            }

            var (success, userType) = _authService.ValidateCredentials(username, password);

            if (success && userType == "Admin")
            {
                // Store user info in session
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("UserType", userType);
                HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                _logger.LogInformation("Admin {Username} logged in", username);

                return RedirectToAction("Index", "Admin");
            }

            ViewData["ErrorMessage"] = "Ongeldige admin credentials.";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                _logger.LogInformation("User {Username} logged out", username);
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;
            ViewData["Username"] = username;
            ViewData["UserType"] = HttpContext.Session.GetString("UserType");
            ViewData["LoginTime"] = HttpContext.Session.GetString("LoginTime");

            return View();
        }
    }
}
