using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Entities;
using SafeCasino.Shared.DTOs;
using SafeCasino.Shared.Requests;
using SafeCasino.Shared.Responses;

namespace SafeCasino.Api.Controllers
{
    /// <summary>
    /// Controller voor het beheren van casino games
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GamesController> _logger;

        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Haal alle games op met optionele filtering en paginering
        /// </summary>
        /// <param name="categoryId">Filter op categorie ID</param>
        /// <param name="providerId">Filter op provider ID</param>
        /// <param name="isPopular">Filter op populaire games</param>
        /// <param name="isNew">Filter op nieuwe games</param>
        /// <param name="searchTerm">Zoekterm voor game naam</param>
        /// <param name="pageNumber">Pagina nummer (standaard 1)</param>
        /// <param name="pageSize">Aantal items per pagina (standaard 12)</param>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetGames(
            [FromQuery] int? categoryId = null,
            [FromQuery] int? providerId = null,
            [FromQuery] bool? isPopular = null,
            [FromQuery] bool? isNew = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12)
        {
            try
            {
                var query = _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.IsAvailable)
                    .AsQueryable();

                // Filtering
                if (categoryId.HasValue)
                    query = query.Where(g => g.CategoryId == categoryId.Value);

                if (providerId.HasValue)
                    query = query.Where(g => g.ProviderId == providerId.Value);

                if (isPopular.HasValue)
                    query = query.Where(g => g.IsPopular == isPopular.Value);

                if (isNew.HasValue)
                    query = query.Where(g => g.IsNew == isNew.Value);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                    query = query.Where(g => g.Name.Contains(searchTerm) ||
                                           g.Description.Contains(searchTerm));

                // Totaal aantal items voor paginering
                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Paginering
                var games = await query
                    .OrderByDescending(g => g.IsPopular)
                    .ThenByDescending(g => g.PlayCount)
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

                return Ok(ApiResponse<object>.SuccessResponse(
                    result, $"{totalItems} games gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van games");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden bij het ophalen van games", 500));
            }
        }

        /// <summary>
        /// Haal een specifieke game op via ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CasinoGameDto>>> GetGame(int id)
        {
            try
            {
                var game = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Include(g => g.Reviews.Where(r => r.IsApproved))
                        .ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null)
                {
                    return NotFound(ApiResponse<CasinoGameDto>.NotFoundResponse(
                        "Game niet gevonden"));
                }

                var gameDto = new CasinoGameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    ThumbnailUrl = game.ThumbnailUrl,
                    MinimumBet = game.MinimumBet,
                    MaximumBet = game.MaximumBet,
                    RtpPercentage = game.RtpPercentage,
                    IsAvailable = game.IsAvailable,
                    IsNew = game.IsNew,
                    IsPopular = game.IsPopular,
                    CreatedDate = game.CreatedDate,
                    PlayCount = game.PlayCount,
                    CategoryId = game.CategoryId,
                    CategoryName = game.Category.Name,
                    ProviderId = game.ProviderId,
                    ProviderName = game.Provider.Name
                };

                return Ok(ApiResponse<CasinoGameDto>.SuccessResponse(gameDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van game {GameId}", id);
                return StatusCode(500, ApiResponse<CasinoGameDto>.ErrorResponse(
                    "Er is een fout opgetreden bij het ophalen van de game", 500));
            }
        }

        /// <summary>
        /// Haal populaire games op
        /// </summary>
        [HttpGet("popular")]
        public async Task<ActionResult<ApiResponse<List<CasinoGameDto>>>> GetPopularGames(
            [FromQuery] int limit = 8)
        {
            try
            {
                var games = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.IsAvailable && g.IsPopular)
                    .OrderByDescending(g => g.PlayCount)
                    .Take(limit)
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
                    games, $"{games.Count} populaire games gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van populaire games");
                return StatusCode(500, ApiResponse<List<CasinoGameDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal nieuwe games op
        /// </summary>
        [HttpGet("new")]
        public async Task<ActionResult<ApiResponse<List<CasinoGameDto>>>> GetNewGames(
            [FromQuery] int limit = 8)
        {
            try
            {
                var games = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.IsAvailable && g.IsNew)
                    .OrderByDescending(g => g.CreatedDate)
                    .Take(limit)
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
                    games, $"{games.Count} nieuwe games gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van nieuwe games");
                return StatusCode(500, ApiResponse<List<CasinoGameDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Maak een nieuwe game aan (alleen Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CasinoGameDto>>> CreateGame(
            [FromBody] CreateGameRequest request)
        {
            try
            {
                // Valideer of categorie en provider bestaan
                var categoryExists = await _context.GameCategories
                    .AnyAsync(c => c.Id == request.CategoryId);
                if (!categoryExists)
                {
                    return BadRequest(ApiResponse<CasinoGameDto>.ErrorResponse(
                        "Ongeldig categorie ID"));
                }

                var providerExists = await _context.GameProviders
                    .AnyAsync(p => p.Id == request.ProviderId);
                if (!providerExists)
                {
                    return BadRequest(ApiResponse<CasinoGameDto>.ErrorResponse(
                        "Ongeldig provider ID"));
                }

                var game = new CasinoGame
                {
                    Name = request.Name,
                    Description = request.Description,
                    ThumbnailUrl = request.ThumbnailUrl,
                    MinimumBet = request.MinimumBet,
                    MaximumBet = request.MaximumBet,
                    RtpPercentage = request.RtpPercentage,
                    IsAvailable = request.IsAvailable,
                    IsNew = request.IsNew,
                    IsPopular = request.IsPopular,
                    CategoryId = request.CategoryId,
                    ProviderId = request.ProviderId,
                    CreatedDate = DateTime.Now,
                    PlayCount = 0
                };

                _context.CasinoGames.Add(game);
                await _context.SaveChangesAsync();

                // Laad relaties voor DTO
                await _context.Entry(game).Reference(g => g.Category).LoadAsync();
                await _context.Entry(game).Reference(g => g.Provider).LoadAsync();

                var gameDto = new CasinoGameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    ThumbnailUrl = game.ThumbnailUrl,
                    MinimumBet = game.MinimumBet,
                    MaximumBet = game.MaximumBet,
                    RtpPercentage = game.RtpPercentage,
                    IsAvailable = game.IsAvailable,
                    IsNew = game.IsNew,
                    IsPopular = game.IsPopular,
                    CreatedDate = game.CreatedDate,
                    PlayCount = game.PlayCount,
                    CategoryId = game.CategoryId,
                    CategoryName = game.Category.Name,
                    ProviderId = game.ProviderId,
                    ProviderName = game.Provider.Name
                };

                _logger.LogInformation("Nieuwe game aangemaakt: {GameName} (ID: {GameId})",
                    game.Name, game.Id);

                return CreatedAtAction(nameof(GetGame), new { id = game.Id },
                    ApiResponse<CasinoGameDto>.SuccessResponse(
                        gameDto, "Game succesvol aangemaakt", 201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij aanmaken van game");
                return StatusCode(500, ApiResponse<CasinoGameDto>.ErrorResponse(
                    "Er is een fout opgetreden bij het aanmaken van de game", 500));
            }
        }

        /// <summary>
        /// Update een bestaande game (alleen Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CasinoGameDto>>> UpdateGame(
            int id, [FromBody] CreateGameRequest request)
        {
            try
            {
                var game = await _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null)
                {
                    return NotFound(ApiResponse<CasinoGameDto>.NotFoundResponse(
                        "Game niet gevonden"));
                }

                // Valideer categorie en provider
                var categoryExists = await _context.GameCategories
                    .AnyAsync(c => c.Id == request.CategoryId);
                if (!categoryExists)
                {
                    return BadRequest(ApiResponse<CasinoGameDto>.ErrorResponse(
                        "Ongeldig categorie ID"));
                }

                var providerExists = await _context.GameProviders
                    .AnyAsync(p => p.Id == request.ProviderId);
                if (!providerExists)
                {
                    return BadRequest(ApiResponse<CasinoGameDto>.ErrorResponse(
                        "Ongeldig provider ID"));
                }

                // Update game properties
                game.Name = request.Name;
                game.Description = request.Description;
                game.ThumbnailUrl = request.ThumbnailUrl;
                game.MinimumBet = request.MinimumBet;
                game.MaximumBet = request.MaximumBet;
                game.RtpPercentage = request.RtpPercentage;
                game.IsAvailable = request.IsAvailable;
                game.IsNew = request.IsNew;
                game.IsPopular = request.IsPopular;
                game.CategoryId = request.CategoryId;
                game.ProviderId = request.ProviderId;

                await _context.SaveChangesAsync();

                // Herlaad relaties voor DTO
                await _context.Entry(game).Reference(g => g.Category).LoadAsync();
                await _context.Entry(game).Reference(g => g.Provider).LoadAsync();

                var gameDto = new CasinoGameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    ThumbnailUrl = game.ThumbnailUrl,
                    MinimumBet = game.MinimumBet,
                    MaximumBet = game.MaximumBet,
                    RtpPercentage = game.RtpPercentage,
                    IsAvailable = game.IsAvailable,
                    IsNew = game.IsNew,
                    IsPopular = game.IsPopular,
                    CreatedDate = game.CreatedDate,
                    PlayCount = game.PlayCount,
                    CategoryId = game.CategoryId,
                    CategoryName = game.Category.Name,
                    ProviderId = game.ProviderId,
                    ProviderName = game.Provider.Name
                };

                _logger.LogInformation("Game bijgewerkt: {GameName} (ID: {GameId})",
                    game.Name, game.Id);

                return Ok(ApiResponse<CasinoGameDto>.SuccessResponse(
                    gameDto, "Game succesvol bijgewerkt"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij bijwerken van game {GameId}", id);
                return StatusCode(500, ApiResponse<CasinoGameDto>.ErrorResponse(
                    "Er is een fout opgetreden bij het bijwerken van de game", 500));
            }
        }

        /// <summary>
        /// Verwijder een game (alleen Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteGame(int id)
        {
            try
            {
                var game = await _context.CasinoGames.FindAsync(id);

                if (game == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Game niet gevonden"));
                }

                _context.CasinoGames.Remove(game);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Game verwijderd: {GameName} (ID: {GameId})",
                    game.Name, game.Id);

                return Ok(ApiResponse.SuccessResponse("Game succesvol verwijderd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van game {GameId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden bij het verwijderen van de game", 500));
            }
        }

        /// <summary>
        /// Verhoog play count van een game (wordt aangeroepen wanneer iemand het spel start)
        /// </summary>
        [Authorize]
        [HttpPost("{id}/play")]
        public async Task<ActionResult<ApiResponse>> IncrementPlayCount(int id)
        {
            try
            {
                var game = await _context.CasinoGames.FindAsync(id);

                if (game == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Game niet gevonden"));
                }

                game.PlayCount++;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse.SuccessResponse("Play count bijgewerkt"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij bijwerken play count voor game {GameId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal alle game categorieën op
        /// </summary>
        [HttpGet("categories")]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetCategories()
        {
            try
            {
                var categories = await _context.GameCategories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.DisplayOrder)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        IconUrl = c.IconUrl,
                        IsActive = c.IsActive,
                        DisplayOrder = c.DisplayOrder,
                        GameCount = c.Games.Count(g => g.IsAvailable)
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<CategoryDto>>.SuccessResponse(
                    categories, $"{categories.Count} categorieën gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van categorieën");
                return StatusCode(500, ApiResponse<List<CategoryDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal games op per categorie
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<object>>> GetGamesByCategory(
            int categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12)
        {
            try
            {
                var category = await _context.GameCategories.FindAsync(categoryId);
                if (category == null)
                {
                    return NotFound(ApiResponse<object>.NotFoundResponse(
                        "Categorie niet gevonden"));
                }

                var query = _context.CasinoGames
                    .Include(g => g.Category)
                    .Include(g => g.Provider)
                    .Where(g => g.CategoryId == categoryId && g.IsAvailable);

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var games = await query
                    .OrderByDescending(g => g.IsPopular)
                    .ThenByDescending(g => g.PlayCount)
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
                    Category = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                        IconUrl = category.IconUrl,
                        IsActive = category.IsActive,
                        DisplayOrder = category.DisplayOrder,
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
                _logger.LogError(ex, "Fout bij ophalen van games voor categorie {CategoryId}",
                    categoryId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }
    }
}