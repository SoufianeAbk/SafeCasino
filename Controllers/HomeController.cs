using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SafeCasino.Models;
using SafeCasino.Services;

namespace SafeCasino.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameApiService _gameApiService;
        private readonly ILocalizationService _localizationService;

        public HomeController(
            ILogger<HomeController> logger,
            IGameApiService gameApiService,
            ILocalizationService localizationService)
        {
            _logger = logger;
            _gameApiService = gameApiService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            // Get featured games for home page
            ViewData["PopularGames"] = await _gameApiService.GetPopularGamesAsync(6);
            ViewData["NewGames"] = await _gameApiService.GetNewGamesAsync(6);
            ViewData["JackpotGames"] = await _gameApiService.GetJackpotGamesAsync(3);

            return View();
        }

        public IActionResult Privacy()
        {
            var language = Request.Cookies["language"] ?? "nl";
            ViewData["Translations"] = _localizationService.GetAllStrings(language);
            ViewData["CurrentLanguage"] = language;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}