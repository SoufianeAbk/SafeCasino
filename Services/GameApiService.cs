using Microsoft.EntityFrameworkCore;
using SafeCasino.Data;
using SafeCasino.Models;

namespace SafeCasino.Services
{
    public class GameApiService : IGameApiService
    {
        private readonly SafeCasinoDbContext _dbContext;
        private readonly ILogger<GameApiService> _logger;

        public GameApiService(SafeCasinoDbContext dbContext, ILogger<GameApiService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ApiGameResponse> GetGamesAsync(FilterOptions filters)
        {
            try
            {
                var query = _dbContext.Games.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filters.SearchTerm))
                {
                    var searchLower = filters.SearchTerm.ToLower();
                    query = query.Where(g =>
                        g.Name.ToLower().Contains(searchLower) ||
                        g.Description.ToLower().Contains(searchLower));
                }

                if (filters.Category.HasValue)
                {
                    query = query.Where(g => g.Category == filters.Category.Value);
                }

                if (!string.IsNullOrEmpty(filters.Provider))
                {
                    query = query.Where(g => g.Provider == filters.Provider);
                }

                if (filters.MinBet.HasValue)
                {
                    query = query.Where(g => g.MinBet >= filters.MinBet.Value);
                }

                if (filters.MaxBet.HasValue)
                {
                    query = query.Where(g => g.MaxBet <= filters.MaxBet.Value);
                }

                if (filters.HasJackpot.HasValue)
                {
                    query = query.Where(g => g.HasJackpot == filters.HasJackpot.Value);
                }

                if (filters.IsNew.HasValue)
                {
                    query = query.Where(g => g.IsNew == filters.IsNew.Value);
                }

                if (filters.IsPopular.HasValue)
                {
                    query = query.Where(g => g.IsPopular == filters.IsPopular.Value);
                }

                // Apply sorting
                query = filters.SortBy switch
                {
                    "RTP" => filters.SortDescending ? query.OrderByDescending(g => g.RTP) : query.OrderBy(g => g.RTP),
                    "Popular" => query.OrderByDescending(g => g.IsPopular),
                    "New" => query.OrderByDescending(g => g.AddedDate),
                    _ => filters.SortDescending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name)
                };

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply pagination
                var games = await query
                    .Skip((filters.PageNumber - 1) * filters.PageSize)
                    .Take(filters.PageSize)
                    .ToListAsync();

                return new ApiGameResponse
                {
                    Success = true,
                    Message = "Games retrieved successfully",
                    Games = games,
                    TotalCount = totalCount,
                    PageNumber = filters.PageNumber,
                    PageSize = filters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving games");
                return new ApiGameResponse
                {
                    Success = false,
                    Message = "Error retrieving games",
                    Games = new List<Game>(),
                    TotalCount = 0
                };
            }
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving game by ID: {GameId}", id);
                return null;
            }
        }

        public async Task<List<Game>> GetPopularGamesAsync(int count = 10)
        {
            try
            {
                return await _dbContext.Games
                    .Where(g => g.IsPopular)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving popular games");
                return new List<Game>();
            }
        }

        public async Task<List<Game>> GetNewGamesAsync(int count = 10)
        {
            try
            {
                return await _dbContext.Games
                    .Where(g => g.IsNew)
                    .OrderByDescending(g => g.AddedDate)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving new games");
                return new List<Game>();
            }
        }

        public async Task<List<Game>> GetJackpotGamesAsync(int count = 10)
        {
            try
            {
                return await _dbContext.Games
                    .Where(g => g.HasJackpot)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving jackpot games");
                return new List<Game>();
            }
        }

        public async Task<List<Game>> GetRelatedGamesAsync(int gameId, int count = 5)
        {
            try
            {
                var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == gameId);
                if (game == null) return new List<Game>();

                return await _dbContext.Games
                    .Where(g => g.Id != gameId && g.Category == game.Category)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving related games for ID: {GameId}", gameId);
                return new List<Game>();
            }
        }

        public async Task<List<string>> GetProvidersAsync()
        {
            try
            {
                return await _dbContext.Games
                    .Select(g => g.Provider)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving providers");
                return new List<string>();
            }
        }

        public async Task<List<GameCategory>> GetCategoriesAsync()
        {
            try
            {
                return await _dbContext.Games
                    .Select(g => g.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return new List<GameCategory>();
            }
        }
    }
}