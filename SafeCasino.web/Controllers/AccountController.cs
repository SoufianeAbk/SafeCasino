using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeCasino.Api.Services;
using SafeCasino.Data.Identity;
using SafeCasino.Web.ViewModels;
using System.Web;

namespace SafeCasino.Web.Controllers
{
    /// <summary>
    /// Controller voor account management en authenticatie
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Login pagina
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Login POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            // Check of email geverifieerd is
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Je moet eerst je e-mailadres verifiëren. Check je inbox.");
                ViewData["ShowResendLink"] = true;
                ViewData["Email"] = model.Email;
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                return RedirectToLocal(model.ReturnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Account is tijdelijk vergrendeld");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Ongeldige login poging");
            return View(model);
        }

        /// <summary>
        /// Registratie pagina
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registratie POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Leeftijdscheck (minimaal 18 jaar)
            var age = DateTime.Today.Year - model.DateOfBirth.Year;
            if (model.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Je moet minimaal 18 jaar oud zijn om te registreren");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                RegistrationDate = DateTime.Now,
                Balance = 0m, // Bonus komt pas na verificatie
                IsVerified = false,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully: {Email}", user.Email);

                // Voeg Player rol toe
                await _userManager.AddToRoleAsync(user, "Player");

                // ✨ Genereer email verificatie token
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                try
                {
                    // ✨ Verstuur verificatie email
                    await _emailService.SendEmailVerificationAsync(user, emailToken);

                    TempData["Success"] = "Registratie succesvol! We hebben een verificatielink naar je e-mailadres gestuurd.";
                    TempData["Email"] = user.Email;

                    return RedirectToAction(nameof(RegisterSuccess));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fout bij verzenden verificatie email naar {Email}", user.Email);
                    TempData["Warning"] = "Account is aangemaakt, maar er ging iets mis bij het verzenden van de verificatie email.";
                    return RedirectToAction(nameof(RegisterSuccess));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        /// <summary>
        /// Registratie succes pagina
        /// </summary>
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        /// <summary>
        /// Verifieer email adres
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Ongeldige verificatie link";
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Gebruiker niet gevonden";
                return RedirectToAction("Login");
            }

            if (user.EmailConfirmed)
            {
                TempData["Info"] = "Je email is al geverifieerd";
                return RedirectToAction("Login");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                // Markeer gebruiker als geverifieerd
                user.IsVerified = true;
                user.Balance = 100m; // ✨ Welkomstbonus na verificatie
                await _userManager.UpdateAsync(user);

                try
                {
                    // Verstuur welkomst email
                    await _emailService.SendWelcomeEmailAsync(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fout bij verzenden welkomst email naar {Email}", user.Email);
                }

                _logger.LogInformation("Email geverifieerd voor gebruiker: {Email}", user.Email);

                TempData["Success"] = "Je email is succesvol geverifieerd! Je ontvangt €100 welkomstbonus. Je kunt nu inloggen.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Email verificatie mislukt. De link is mogelijk verlopen.";
            ViewData["Email"] = user.Email;
            return View("VerificationFailed");
        }

        /// <summary>
        /// Verstuur verificatie email opnieuw
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerification(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Info"] = "Als het account bestaat, is er een verificatie email verstuurd";
                return RedirectToAction("Login");
            }

            if (user.EmailConfirmed)
            {
                TempData["Info"] = "Je email is al geverifieerd";
                return RedirectToAction("Login");
            }

            try
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailService.SendEmailVerificationAsync(user, emailToken);

                TempData["Success"] = "Verificatie email is opnieuw verstuurd. Check je inbox.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij opnieuw versturen verificatie email naar {Email}", email);
                TempData["Error"] = "Er is een fout opgetreden bij het verzenden van de email";
            }

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Logout
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Profiel pagina
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Access denied pagina
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}