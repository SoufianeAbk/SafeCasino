using Microsoft.AspNetCore.Mvc;
using SafeCasino.Services;

namespace SafeCasino.Controllers
{
    public class FAQController : Controller
    {
        private readonly ILocalizationService _localizationService;

        public FAQController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public IActionResult Index()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            return View();
        }
    }
}