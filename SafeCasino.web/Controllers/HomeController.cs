using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SafeCasino.Data.Data;
using SafeCasino.web.Models;
using System.Diagnostics;

namespace SafeCasino.web.Controllers
{
    /// <summary>
    /// Controller voor de homepage en algemene pagina's
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Homepage met populaire en nieuwe games (met caching voor betere performance)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cache populaire games voor 5 minuten
                var popularGames = await _cache.GetOrCreateAsync("PopularGames", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                    _logger.LogInformation("Loading popular games from database");

                    return await _context.CasinoGames
                        .AsNoTracking() // Performance boost - read-only
                        .Include(g => g.Provider)
                        .Include(g => g.Category)
                        .Where(g => g.IsAvailable && g.IsPopular)
                        .OrderByDescending(g => g.PlayCount)
                        .Take(10)
                        .ToListAsync();
                });

                // Cache nieuwe games voor 5 minuten
                var newGames = await _cache.GetOrCreateAsync("NewGames", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                    _logger.LogInformation("Loading new games from database");

                    return await _context.CasinoGames
                        .AsNoTracking()
                        .Include(g => g.Provider)
                        .Include(g => g.Category)
                        .Where(g => g.IsAvailable && g.IsNew)
                        .OrderByDescending(g => g.CreatedDate)
                        .Take(10)
                        .ToListAsync();
                });

                // Cache categorieën voor 10 minuten (verandert zelden)
                var categories = await _cache.GetOrCreateAsync("Categories", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                    _logger.LogInformation("Loading categories from database");

                    return await _context.GameCategories
                        .AsNoTracking()
                        .Where(c => c.IsActive)
                        .OrderBy(c => c.DisplayOrder)
                        .ToListAsync();
                });

                ViewBag.PopularGames = popularGames ?? new List<SafeCasino.Data.Entities.CasinoGame>();
                ViewBag.NewGames = newGames ?? new List<SafeCasino.Data.Entities.CasinoGame>();
                ViewBag.Categories = categories ?? new List<SafeCasino.Data.Entities.GameCategory>();

                return View();
            }
            catch (Exception ex)
            {
                // Log de fout maar laat de site niet crashen
                _logger.LogError(ex, "Error loading homepage data");

                // Lege data tonen als fallback
                ViewBag.PopularGames = new List<SafeCasino.Data.Entities.CasinoGame>();
                ViewBag.NewGames = new List<SafeCasino.Data.Entities.CasinoGame>();
                ViewBag.Categories = new List<SafeCasino.Data.Entities.GameCategory>();

                return View();
            }
        }

        /// <summary>
        /// Tournament informatie pagina met details over de dagelijkse prijzenpot
        /// </summary>
        public IActionResult TournamentInfo()
        {
            _logger.LogInformation("Tournament info page accessed");
            return View();
        }

        /// <summary>
        /// Privacy policy pagina
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error pagina
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}