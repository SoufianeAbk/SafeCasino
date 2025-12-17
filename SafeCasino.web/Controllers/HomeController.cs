using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.web.Models;
using System.Diagnostics;

namespace SafeCasino.Web.Controllers
{
    /// <summary>
    /// Controller voor de home pagina en algemene informatie
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
        /// Home pagina
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
        /// Tournament info pagina
        /// </summary>
        public IActionResult TournamentInfo()
        {
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
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}