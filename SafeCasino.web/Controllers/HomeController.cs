using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.web.Models;

namespace SafeCasino.web.Controllers
{
    /// <summary>
    /// Controller voor de hoofdpagina en algemene pagina's
    /// </summary>
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
        /// Homepage met overzicht van populaire en nieuwe games
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Haal populaire games op
            var popularGames = await _context.CasinoGames
                .Include(g => g.Provider)
                .Include(g => g.Category)
                .Where(g => g.IsAvailable && g.IsPopular)
                .OrderByDescending(g => g.PlayCount)
                .Take(10)
                .ToListAsync();

            // Haal nieuwe games op
            var newGames = await _context.CasinoGames
                .Include(g => g.Provider)
                .Include(g => g.Category)
                .Where(g => g.IsAvailable && g.IsNew)
                .OrderByDescending(g => g.CreatedDate)
                .Take(10)
                .ToListAsync();

            // Haal actieve categorieën op
            var categories = await _context.GameCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            // Geef data door aan view via ViewBag
            ViewBag.PopularGames = popularGames;
            ViewBag.NewGames = newGames;
            ViewBag.Categories = categories;

            return View();
        }

        /// <summary>
        /// Privacy pagina
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