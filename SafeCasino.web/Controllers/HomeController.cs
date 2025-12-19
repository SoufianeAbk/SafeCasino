using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.web.Models;
using System.Diagnostics;

namespace SafeCasino.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Home pagina met populaire en nieuwe games
        /// </summary>
        public async Task<IActionResult> Index()
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

        /// <summary>
        /// Privacy Policy pagina
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
        /// Over Ons pagina - informatie over SafeCasino
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Verantwoord Spelen pagina - informatie over verantwoord gokken
        /// </summary>
        public IActionResult ResponsibleGaming()
        {
            return View();
        }

        /// <summary>
        /// Algemene Voorwaarden pagina
        /// </summary>
        public IActionResult Terms()
        {
            return View();
        }

        /// <summary>
        /// Contact pagina - contactgegevens en formulier
        /// </summary>
        public IActionResult Contact()
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