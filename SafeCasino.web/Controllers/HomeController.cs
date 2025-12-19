using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;

namespace SafeCasino.Web.Controllers
{
    /// <summary>
    /// Controller voor de homepage en informatieve pagina's
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Homepage met populaire en nieuwe games
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Haal populaire games op
                var popularGames = await _context.CasinoGames
                    .Include(g => g.Provider)
                    .Include(g => g.Category)
                    .Where(g => g.IsPopular && g.IsAvailable)
                    .OrderByDescending(g => g.PlayCount)
                    .Take(10)
                    .ToListAsync();

                // Haal nieuwe games op
                var newGames = await _context.CasinoGames
                    .Include(g => g.Provider)
                    .Include(g => g.Category)
                    .Where(g => g.IsNew && g.IsAvailable)
                    .OrderByDescending(g => g.CreatedDate)
                    .Take(10)
                    .ToListAsync();

                // Haal categorieën op
                var categories = await _context.GameCategories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.DisplayOrder)
                    .ToListAsync();

                ViewBag.PopularGames = popularGames;
                ViewBag.NewGames = newGames;
                ViewBag.Categories = categories;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het laden van homepage");
                return View();
            }
        }

        /// <summary>
        /// Over ons pagina
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Contact pagina
        /// </summary>
        public IActionResult Contact()
        {
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
        /// Tournament informatie pagina
        /// </summary>
        public IActionResult TournamentInfo()
        {
            return View();
        }

        /// <summary>
        /// Verantwoord spelen informatie pagina
        /// </summary>
        public IActionResult ResponsibleGaming()
        {
            return View();
        }

        /// <summary>
        /// Error pagina
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}