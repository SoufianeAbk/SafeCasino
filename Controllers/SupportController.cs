using Microsoft.AspNetCore.Mvc;
using SafeCasino.Services;

namespace SafeCasino.Controllers
{
    public class SupportController : Controller
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<SupportController> _logger;

        public SupportController(
            ILocalizationService localizationService,
            ILogger<SupportController> logger)
        {
            _localizationService = localizationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            return View();
        }

        [HttpPost]
        public IActionResult SubmitTicket(string name, string email, string subject, string message)
        {
            // In een echte applicatie zou je hier het ticket opslaan
            _logger.LogInformation("Support ticket submitted by {Name} ({Email}): {Subject}", name, email, subject);

            TempData["SuccessMessage"] = "Uw bericht is verzonden. We nemen zo snel mogelijk contact met u op.";
            return RedirectToAction(nameof(Index));
        }
    }
}