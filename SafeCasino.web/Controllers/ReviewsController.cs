using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Entities;
using SafeCasino.Web.ViewModels;
using SafeCasino.Data.Identity;

namespace SafeCasino.Web.Controllers
{
    /// <summary>
    /// Controller voor game reviews
    /// </summary>
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ReviewsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Create review GET
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(int gameId)
        {
            var game = await _context.CasinoGames.FindAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            // Check of gebruiker al een review heeft geschreven
            var userId = _userManager.GetUserId(User);
            var existingReview = await _context.Reviews
                .AnyAsync(r => r.GameId == gameId && r.UserId == userId);

            if (existingReview)
            {
                TempData["Error"] = "Je hebt al een review geschreven voor dit spel";
                return RedirectToAction("Details", "Games", new { id = gameId });
            }

            var viewModel = new CreateReviewViewModel
            {
                GameId = gameId,
                GameName = game.Name
            };

            return View(viewModel);
        }

        /// <summary>
        /// Create review POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            // Check nogmaals of er al een review bestaat
            var existingReview = await _context.Reviews
                .AnyAsync(r => r.GameId == model.GameId && r.UserId == userId);

            if (existingReview)
            {
                TempData["Error"] = "Je hebt al een review geschreven voor dit spel";
                return RedirectToAction("Details", "Games", new { id = model.GameId });
            }

            var review = new Review
            {
                GameId = model.GameId,
                UserId = userId!,
                Title = model.Title,
                Content = model.Content,
                Rating = model.Rating,
                CreatedDate = DateTime.Now,
                IsApproved = false // Reviews moeten eerst goedgekeurd worden
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Je review is ingediend en wacht op goedkeuring";
            return RedirectToAction("Details", "Games", new { id = model.GameId });
        }

        /// <summary>
        /// Edit review GET
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            // Check of review van huidige gebruiker is
            var userId = _userManager.GetUserId(User);
            if (review.UserId != userId)
            {
                return Forbid();
            }

            var viewModel = new CreateReviewViewModel
            {
                GameId = review.GameId,
                GameName = review.Game.Name,
                Title = review.Title,
                Content = review.Content,
                Rating = review.Rating
            };

            ViewBag.ReviewId = id;
            return View(viewModel);
        }

        /// <summary>
        /// Edit review POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReviewId = id;
                return View(model);
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Check of review van huidige gebruiker is
            var userId = _userManager.GetUserId(User);
            if (review.UserId != userId)
            {
                return Forbid();
            }

            review.Title = model.Title;
            review.Content = model.Content;
            review.Rating = model.Rating;
            review.IsApproved = false; // Reset approval status bij edit

            await _context.SaveChangesAsync();

            TempData["Success"] = "Je review is bijgewerkt en wacht op goedkeuring";
            return RedirectToAction("Details", "Games", new { id = review.GameId });
        }

        /// <summary>
        /// Delete review
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            // Check of review van huidige gebruiker is
            var userId = _userManager.GetUserId(User);
            if (review.UserId != userId)
            {
                return Forbid();
            }

            var gameId = review.GameId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Je review is verwijderd";
            return RedirectToAction("Details", "Games", new { id = gameId });
        }
    }
}