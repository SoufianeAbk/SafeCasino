using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Entities;
using SafeCasino.Web.ViewModels;

namespace SafeCasino.Web.Controllers
{
    /// <summary>
    /// Controller voor casino games
    /// </summary>
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GamesController> _logger;

        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Overzicht van alle games
        /// </summary>
        public async Task<IActionResult> Index(int? categoryId, string? search, string? sortBy)
        {
            var query = _context.CasinoGames
                .Include(g => g.Category)
                .Include(g => g.Provider)
                .Include(g => g.Reviews)
                .Where(g => g.IsAvailable)
                .AsQueryable();

            // Filter op categorie
            if (categoryId.HasValue)
            {
                query = query.Where(g => g.CategoryId == categoryId.Value);
            }

            // Zoeken
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g => g.Name.Contains(search) || g.Description.Contains(search));
            }

            // Sorteren
            query = sortBy switch
            {
                "popular" => query.OrderByDescending(g => g.PlayCount),
                "new" => query.OrderByDescending(g => g.CreatedDate),
                "rtp" => query.OrderByDescending(g => g.RtpPercentage),
                "name" => query.OrderBy(g => g.Name),
                _ => query.OrderByDescending(g => g.IsPopular)
                          .ThenByDescending(g => g.PlayCount)
            };

            var games = await query.ToListAsync();
            var categories = await _context.GameCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var viewModel = new GamesIndexViewModel
            {
                Games = games,
                Categories = categories,
                SelectedCategoryId = categoryId,
                SearchQuery = search,
                SortBy = sortBy,
                TotalGames = games.Count,
                PopularGamesCount = games.Count(g => g.IsPopular),
                NewGamesCount = games.Count(g => g.IsNew)
            };

            return View(viewModel);
        }

        /// <summary>
        /// Details van een specifiek game
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var game = await _context.CasinoGames
                .Include(g => g.Category)
                .Include(g => g.Provider)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var approvedReviews = game.Reviews
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CreatedDate)
                .ToList();

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                Reviews = approvedReviews,

                // ✅ FIX: explicit cast double -> decimal
                AverageRating = approvedReviews.Any()
                    ? (decimal)approvedReviews.Average(r => r.Rating)
                    : 0m,

                TotalReviews = approvedReviews.Count,
                CanReview = User.Identity?.IsAuthenticated ?? false,
                UserHasReviewed = User.Identity?.IsAuthenticated == true &&
                    approvedReviews.Any(r => r.User.UserName == User.Identity.Name)
            };

            return View(viewModel);
        }

        /// <summary>
        /// Games per categorie
        /// </summary>
        public async Task<IActionResult> Category(int id)
        {
            var category = await _context.GameCategories
                .Include(c => c.Games)
                    .ThenInclude(g => g.Provider)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryDescription = category.Description;

            return View(category.Games.Where(g => g.IsAvailable).ToList());
        }

        /// <summary>
        /// Populaire games
        /// </summary>
        public async Task<IActionResult> Popular()
        {
            var games = await _context.CasinoGames
                .Include(g => g.Category)
                .Include(g => g.Provider)
                .Where(g => g.IsAvailable && g.IsPopular)
                .OrderByDescending(g => g.PlayCount)
                .Take(20)
                .ToListAsync();

            ViewBag.Title = "Populaire Games";
            return View("GamesList", games);
        }

        /// <summary>
        /// Nieuwe games
        /// </summary>
        public async Task<IActionResult> New()
        {
            var games = await _context.CasinoGames
                .Include(g => g.Category)
                .Include(g => g.Provider)
                .Where(g => g.IsAvailable && g.IsNew)
                .OrderByDescending(g => g.CreatedDate)
                .Take(20)
                .ToListAsync();

            ViewBag.Title = "Nieuwe Games";
            return View("GamesList", games);
        }

        /// <summary>
        /// Speel een game (placeholder voor game iframe)
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Play(int id)
        {
            var game = await _context.CasinoGames
                .Include(g => g.Category)
                .Include(g => g.Provider)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null || !game.IsAvailable)
            {
                return NotFound();
            }

            // Update play count
            game.PlayCount++;
            await _context.SaveChangesAsync();

            return View(game);
        }
    }
}
