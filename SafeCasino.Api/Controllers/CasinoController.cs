using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Shared.DTOs;
using SafeCasino.Shared.Responses;

namespace SafeCasino.Api.Controllers
{
    /// <summary>
    /// Controller voor casino dashboard en statistieken
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CasinoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CasinoController> _logger;

        public CasinoController(ApplicationDbContext context, ILogger<CasinoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Haal dashboard statistieken op (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<ActionResult<ApiResponse<object>>> GetDashboard()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var totalGames = await _context.CasinoGames.CountAsync();
                var totalReviews = await _context.Reviews.CountAsync();
                var pendingReviews = await _context.Reviews.CountAsync(r => !r.IsApproved);

                // Populaire games
                var popularGames = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .OrderByDescending(g => g.PlayCount)
                    .Take(5)
                    .Select(g => new
                    {
                        g.Id,
                        g.Name,
                        g.ThumbnailUrl,
                        CategoryName = g.Category.Name,
                        ProviderName = g.Provider.Name,
                        g.PlayCount,
                        g.IsPopular
                    })
                    .ToListAsync();

                // Recente reviews
                var recentReviews = await _context.Reviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .OrderByDescending(r => r.CreatedDate)
                    .Take(10)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Content = r.Content,
                        Rating = r.Rating,
                        CreatedDate = r.CreatedDate,
                        IsApproved = r.IsApproved,
                        GameId = r.GameId,
                        GameName = r.Game.Name,
                        UserId = r.UserId,
                        UserName = $"{r.User.FirstName} {r.User.LastName}"
                    })
                    .ToListAsync();

                // Categorie statistieken
                var categoryStats = await _context.GameCategories
                    .Where(c => c.IsActive)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.IconUrl,
                        GameCount = c.Games.Count(g => g.IsAvailable),
                        TotalPlays = c.Games.Sum(g => g.PlayCount)
                    })
                    .OrderByDescending(c => c.TotalPlays)
                    .ToListAsync();

                // Provider statistieken
                var providerStats = await _context.GameProviders
                    .Where(p => p.IsActive)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.LogoUrl,
                        GameCount = p.Games.Count(g => g.IsAvailable),
                        TotalPlays = p.Games.Sum(g => g.PlayCount)
                    })
                    .OrderByDescending(p => p.GameCount)
                    .ToListAsync();

                // Recente gebruikers
                var recentUsers = await _context.Users
                    .OrderByDescending(u => u.RegistrationDate)
                    .Take(10)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        FullName = $"{u.FirstName} {u.LastName}",
                        u.RegistrationDate,
                        u.Balance,
                        u.IsVerified
                    })
                    .ToListAsync();

                var dashboard = new
                {
                    Statistics = new
                    {
                        TotalUsers = totalUsers,
                        TotalGames = totalGames,
                        TotalReviews = totalReviews,
                        PendingReviews = pendingReviews
                    },
                    PopularGames = popularGames,
                    RecentReviews = recentReviews,
                    CategoryStats = categoryStats,
                    ProviderStats = providerStats,
                    RecentUsers = recentUsers
                };

                return Ok(ApiResponse<object>.SuccessResponse(dashboard));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van dashboard");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal reviews op voor een specifieke game
        /// </summary>
        [HttpGet("games/{gameId}/reviews")]
        public async Task<ActionResult<ApiResponse<object>>> GetGameReviews(
            int gameId,
            [FromQuery] bool includeUnapproved = false)
        {
            try
            {
                var game = await _context.CasinoGames.FindAsync(gameId);
                if (game == null)
                {
                    return NotFound(ApiResponse<object>.NotFoundResponse(
                        "Game niet gevonden"));
                }

                var query = _context.Reviews
                    .Include(r => r.User)
                    .Where(r => r.GameId == gameId);

                // Alleen goedgekeurde reviews tonen tenzij admin
                if (!includeUnapproved || !User.IsInRole("Admin"))
                {
                    query = query.Where(r => r.IsApproved);
                }

                var reviews = await query
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Content = r.Content,
                        Rating = r.Rating,
                        CreatedDate = r.CreatedDate,
                        IsApproved = r.IsApproved,
                        GameId = r.GameId,
                        GameName = game.Name,
                        UserId = r.UserId,
                        UserName = $"{r.User.FirstName} {r.User.LastName}"
                    })
                    .ToListAsync();

                // Bereken statistieken
                var averageRating = reviews.Any()
                    ? reviews.Average(r => r.Rating)
                    : 0;

                var ratingDistribution = reviews
                    .GroupBy(r => r.Rating)
                    .Select(g => new { Rating = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Rating)
                    .ToList();

                var result = new
                {
                    Reviews = reviews,
                    Statistics = new
                    {
                        TotalReviews = reviews.Count,
                        AverageRating = Math.Round(averageRating, 2),
                        RatingDistribution = ratingDistribution
                    }
                };

                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van game reviews");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal alle reviews op (Admin/Moderator)
        /// </summary>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("reviews")]
        public async Task<ActionResult<ApiResponse<List<ReviewDto>>>> GetAllReviews(
            [FromQuery] bool? approved = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var query = _context.Reviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .AsQueryable();

                if (approved.HasValue)
                {
                    query = query.Where(r => r.IsApproved == approved.Value);
                }

                var totalItems = await query.CountAsync();
                var reviews = await query
                    .OrderByDescending(r => r.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Content = r.Content,
                        Rating = r.Rating,
                        CreatedDate = r.CreatedDate,
                        IsApproved = r.IsApproved,
                        GameId = r.GameId,
                        GameName = r.Game.Name,
                        UserId = r.UserId,
                        UserName = $"{r.User.FirstName} {r.User.LastName}"
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<ReviewDto>>.SuccessResponse(
                    reviews, $"{totalItems} reviews gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van alle reviews");
                return StatusCode(500, ApiResponse<List<ReviewDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal game providers op
        /// </summary>
        [HttpGet("providers")]
        public async Task<ActionResult<ApiResponse<List<object>>>> GetProviders()
        {
            try
            {
                var providers = await _context.GameProviders
                    .Where(p => p.IsActive)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.LogoUrl,
                        p.WebsiteUrl,
                        GameCount = p.Games.Count(g => g.IsAvailable)
                    })
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                return Ok(ApiResponse<List<object>>.SuccessResponse(
                    providers.Cast<object>().ToList(),
                    $"{providers.Count} providers gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van providers");
                return StatusCode(500, ApiResponse<List<object>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal games per provider op
        /// </summary>
        [HttpGet("providers/{providerId}/games")]
        public async Task<ActionResult<ApiResponse<object>>> GetGamesByProvider(
            int providerId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12)
        {
            try
            {
                var provider = await _context.GameProviders.FindAsync(providerId);
                if (provider == null)
                {
                    return NotFound(ApiResponse<object>.NotFoundResponse(
                        "Provider niet gevonden"));
                }

                var query = _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.ProviderId == providerId && g.IsAvailable);

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var games = await query
                    .OrderByDescending(g => g.PlayCount)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(g => new CasinoGameDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Description = g.Description,
                        ThumbnailUrl = g.ThumbnailUrl,
                        MinimumBet = g.MinimumBet,
                        MaximumBet = g.MaximumBet,
                        RtpPercentage = g.RtpPercentage,
                        IsAvailable = g.IsAvailable,
                        IsNew = g.IsNew,
                        IsPopular = g.IsPopular,
                        CreatedDate = g.CreatedDate,
                        PlayCount = g.PlayCount,
                        CategoryId = g.CategoryId,
                        CategoryName = g.Category.Name,
                        ProviderId = g.ProviderId,
                        ProviderName = g.Provider.Name
                    })
                    .ToListAsync();

                var result = new
                {
                    Provider = new
                    {
                        provider.Id,
                        provider.Name,
                        provider.Description,
                        provider.LogoUrl,
                        provider.WebsiteUrl,
                        GameCount = totalItems
                    },
                    Games = games,
                    Pagination = new
                    {
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalItems = totalItems,
                        TotalPages = totalPages,
                        HasPrevious = pageNumber > 1,
                        HasNext = pageNumber < totalPages
                    }
                };

                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van games voor provider {ProviderId}",
                    providerId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal statistieken op voor analytics (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("analytics")]
        public async Task<ActionResult<ApiResponse<object>>> GetAnalytics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.Now.AddDays(-30);
                var end = endDate ?? DateTime.Now;

                // Nieuwe gebruikers per dag
                var newUsersPerDay = await _context.Users
                    .Where(u => u.RegistrationDate >= start && u.RegistrationDate <= end)
                    .GroupBy(u => u.RegistrationDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                // Totale game plays per categorie
                var playsPerCategory = await _context.GameCategories
                    .Select(c => new
                    {
                        Category = c.Name,
                        TotalPlays = c.Games.Sum(g => g.PlayCount)
                    })
                    .OrderByDescending(x => x.TotalPlays)
                    .ToListAsync();

                // Top games
                var topGames = await _context.CasinoGames
                    .OrderByDescending(g => g.PlayCount)
                    .Take(10)
                    .Select(g => new
                    {
                        g.Id,
                        g.Name,
                        g.PlayCount,
                        Category = g.Category.Name,
                        Provider = g.Provider.Name
                    })
                    .ToListAsync();

                // Review statistieken
                var reviewStats = new
                {
                    TotalReviews = await _context.Reviews.CountAsync(),
                    ApprovedReviews = await _context.Reviews.CountAsync(r => r.IsApproved),
                    PendingReviews = await _context.Reviews.CountAsync(r => !r.IsApproved),
                    AverageRating = await _context.Reviews
                        .Where(r => r.IsApproved)
                        .AverageAsync(r => (double?)r.Rating) ?? 0
                };

                var analytics = new
                {
                    DateRange = new { Start = start, End = end },
                    NewUsersPerDay = newUsersPerDay,
                    PlaysPerCategory = playsPerCategory,
                    TopGames = topGames,
                    ReviewStats = reviewStats
                };

                return Ok(ApiResponse<object>.SuccessResponse(analytics));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van analytics");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Zoek games
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<List<CasinoGameDto>>>> SearchGames(
            [FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(ApiResponse<List<CasinoGameDto>>.ErrorResponse(
                        "Zoekterm is verplicht"));
                }

                var games = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.IsAvailable &&
                               (g.Name.Contains(query) ||
                                g.Description.Contains(query) ||
                                g.Category.Name.Contains(query) ||
                                g.Provider.Name.Contains(query)))
                    .OrderByDescending(g => g.IsPopular)
                    .ThenByDescending(g => g.PlayCount)
                    .Take(20)
                    .Select(g => new CasinoGameDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Description = g.Description,
                        ThumbnailUrl = g.ThumbnailUrl,
                        MinimumBet = g.MinimumBet,
                        MaximumBet = g.MaximumBet,
                        RtpPercentage = g.RtpPercentage,
                        IsAvailable = g.IsAvailable,
                        IsNew = g.IsNew,
                        IsPopular = g.IsPopular,
                        CreatedDate = g.CreatedDate,
                        PlayCount = g.PlayCount,
                        CategoryId = g.CategoryId,
                        CategoryName = g.Category.Name,
                        ProviderId = g.ProviderId,
                        ProviderName = g.Provider.Name
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<CasinoGameDto>>.SuccessResponse(
                    games, $"{games.Count} resultaten gevonden voor '{query}'"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij zoeken naar games");
                return StatusCode(500, ApiResponse<List<CasinoGameDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }
    }
}