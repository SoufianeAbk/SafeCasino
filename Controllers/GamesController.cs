using Microsoft.AspNetCore.Mvc;
using SafeCasino.Models;
using SafeCasino.Services;
using SafeCasino.ViewModels;

namespace SafeCasino.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameApiService _gameApiService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(
            IGameApiService gameApiService,
            ILocalizationService localizationService,
            ILogger<GamesController> logger)
        {
            _gameApiService = gameApiService;
            _localizationService = localizationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            string? category = null,
            string? provider = null,
            decimal? minBet = null,
            decimal? maxBet = null,
            bool? hasJackpot = null,
            bool? isNew = null,
            bool? isPopular = null,
            string sortBy = "Name",
            bool sortDescending = false,
            int pageNumber = 1,
            int pageSize = 20)
        {
            try
            {
                // Parse category string to enum
                GameCategory? categoryEnum = null;
                if (!string.IsNullOrEmpty(category) && Enum.TryParse<GameCategory>(category, true, out var parsed))
                {
                    categoryEnum = parsed;
                }

                var filters = new FilterOptions
                {
                    SearchTerm = searchTerm,
                    Category = categoryEnum,
                    Provider = provider,
                    MinBet = minBet,
                    MaxBet = maxBet,
                    HasJackpot = hasJackpot,
                    IsNew = isNew,
                    IsPopular = isPopular,
                    SortBy = sortBy,
                    SortDescending = sortDescending,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var response = await _gameApiService.GetGamesAsync(filters);
                var providers = await _gameApiService.GetProvidersAsync();
                var categories = await _gameApiService.GetCategoriesAsync();

                var language = Request.Cookies["language"] ?? "nl";
                ViewData["Translations"] = _localizationService.GetAllStrings(language);
                ViewData["CurrentLanguage"] = language;

                var viewModel = new GameListViewModel
                {
                    Games = response.Games,
                    Filters = filters,
                    AvailableProviders = providers,
                    AvailableCategories = categories,
                    TotalGames = response.TotalCount,
                    CurrentPage = response.PageNumber,
                    TotalPages = response.TotalPages,
                    CurrentLanguage = language
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading games");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var game = await _gameApiService.GetGameByIdAsync(id);
                if (game == null)
                {
                    return NotFound();
                }

                var relatedGames = await _gameApiService.GetRelatedGamesAsync(id);
                var language = Request.Cookies["language"] ?? "nl";
                ViewData["Translations"] = _localizationService.GetAllStrings(language);
                ViewData["CurrentLanguage"] = language;

                var viewModel = new GameDetailViewModel
                {
                    Game = game,
                    RelatedGames = relatedGames,
                    CurrentLanguage = language
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading game details for ID: {GameId}", id);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Popular()
        {
            try
            {
                var games = await _gameApiService.GetPopularGamesAsync(20);
                var language = Request.Cookies["language"] ?? "nl";
                ViewData["Translations"] = _localizationService.GetAllStrings(language);
                ViewData["CurrentLanguage"] = language;

                var viewModel = new GameListViewModel
                {
                    Games = games,
                    CurrentLanguage = language
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading popular games");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            try
            {
                var games = await _gameApiService.GetNewGamesAsync(20);
                var language = Request.Cookies["language"] ?? "nl";
                ViewData["Translations"] = _localizationService.GetAllStrings(language);
                ViewData["CurrentLanguage"] = language;

                var viewModel = new GameListViewModel
                {
                    Games = games,
                    CurrentLanguage = language
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading new games");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Jackpot()
        {
            try
            {
                var games = await _gameApiService.GetJackpotGamesAsync(20);
                var language = Request.Cookies["language"] ?? "nl";
                ViewData["Translations"] = _localizationService.GetAllStrings(language);
                ViewData["CurrentLanguage"] = language;

                var viewModel = new GameListViewModel
                {
                    Games = games,
                    CurrentLanguage = language
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading jackpot games");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Play(int id, bool demo = true)
        {
            // In een echte applicatie zou je hier naar het spel navigeren
            // Voor nu redirect naar details
            TempData["PlayMode"] = demo ? "demo" : "real";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
