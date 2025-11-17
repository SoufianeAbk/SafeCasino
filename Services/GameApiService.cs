using SafeCasino.Models;

namespace SafeCasino.Services
{
    public class GameApiService : IGameApiService
    {
        private readonly List<Game> _mockGames;

        public GameApiService()
        {
            _mockGames = GenerateMockGames();
        }

        private List<Game> GenerateMockGames()
        {
            var games = new List<Game>();
            var providers = new[] { "NetEnt", "Microgaming", "PlayTech", "Evolution", "Pragmatic Play", "Novomatic" };
            var slotNames = new[] { "Book of Ra", "Starburst", "Gonzo's Quest", "Mega Moolah", "Dead or Alive",
                                    "Bonanza", "Sweet Bonanza", "Gates of Olympus", "Wolf Gold", "Great Rhino" };
            var tableNames = new[] { "European Roulette", "American Roulette", "Blackjack Pro", "Baccarat Gold",
                                     "Casino Hold'em", "Three Card Poker", "Caribbean Stud" };
            var liveNames = new[] { "Live Roulette", "Live Blackjack", "Live Baccarat", "Live Dream Catcher",
                                   "Live Monopoly", "Live Crazy Time", "Live Lightning Roulette" };

            int id = 1;

            // Generate Slots
            foreach (var name in slotNames)
            {
                games.Add(new Game
                {
                    Id = id++,
                    Name = name,
                    Description = $"Spannende slot {name} met geweldige bonussen!",
                    Provider = providers[Random.Shared.Next(providers.Length)],
                    ThumbnailUrl = $"https://picsum.photos/300/200?random={id}",
                    GameUrl = $"/play/{id}",
                    Category = GameCategory.Slots,
                    MinBet = 0.10m,
                    MaxBet = 100m,
                    RTP = 94m + (decimal)(Random.Shared.NextDouble() * 4),
                    IsPopular = Random.Shared.Next(100) > 70,
                    IsNew = Random.Shared.Next(100) > 80,
                    HasJackpot = Random.Shared.Next(100) > 75,
                    AddedDate = DateTime.Now.AddDays(-Random.Shared.Next(365)),
                    Tags = new List<string> { "Bonus", "Free Spins", "Wild" }
                });
            }

            // Generate Table Games
            foreach (var name in tableNames)
            {
                games.Add(new Game
                {
                    Id = id++,
                    Name = name,
                    Description = $"Klassiek tafelspel {name} voor echte casino liefhebbers!",
                    Provider = providers[Random.Shared.Next(providers.Length)],
                    ThumbnailUrl = $"https://picsum.photos/300/200?random={id}",
                    GameUrl = $"/play/{id}",
                    Category = name.Contains("Roulette") ? GameCategory.Roulette :
                             name.Contains("Blackjack") ? GameCategory.Blackjack :
                             name.Contains("Baccarat") ? GameCategory.Baccarat :
                             GameCategory.TableGames,
                    MinBet = 1m,
                    MaxBet = 500m,
                    RTP = 97m + (decimal)(Random.Shared.NextDouble() * 2),
                    IsPopular = Random.Shared.Next(100) > 60,
                    IsNew = Random.Shared.Next(100) > 90,
                    HasJackpot = false,
                    AddedDate = DateTime.Now.AddDays(-Random.Shared.Next(365)),
                    Tags = new List<string> { "Strategie", "Klassiek" }
                });
            }

            // Generate Live Casino Games
            foreach (var name in liveNames)
            {
                games.Add(new Game
                {
                    Id = id++,
                    Name = name,
                    Description = $"Live casino ervaring met {name} en echte dealers!",
                    Provider = "Evolution",
                    ThumbnailUrl = $"https://picsum.photos/300/200?random={id}",
                    GameUrl = $"/play/{id}",
                    Category = GameCategory.LiveCasino,
                    MinBet = 5m,
                    MaxBet = 1000m,
                    RTP = 97m + (decimal)(Random.Shared.NextDouble() * 2),
                    IsPopular = Random.Shared.Next(100) > 50,
                    IsNew = Random.Shared.Next(100) > 85,
                    HasJackpot = false,
                    AddedDate = DateTime.Now.AddDays(-Random.Shared.Next(365)),
                    Tags = new List<string> { "Live", "HD Stream", "Dealers" }
                });
            }

            return games;
        }

        public async Task<ApiGameResponse> GetGamesAsync(FilterOptions filters)
        {
            await Task.Delay(100); // Simulate API delay

            var query = _mockGames.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                query = query.Where(g => g.Name.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                        g.Description.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase));
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

            var totalCount = query.Count();
            var games = query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList();

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

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            await Task.Delay(50);
            return _mockGames.FirstOrDefault(g => g.Id == id);
        }

        public async Task<List<Game>> GetPopularGamesAsync(int count = 10)
        {
            await Task.Delay(50);
            return _mockGames.Where(g => g.IsPopular).Take(count).ToList();
        }

        public async Task<List<Game>> GetNewGamesAsync(int count = 10)
        {
            await Task.Delay(50);
            return _mockGames.Where(g => g.IsNew).OrderByDescending(g => g.AddedDate).Take(count).ToList();
        }

        public async Task<List<Game>> GetJackpotGamesAsync(int count = 10)
        {
            await Task.Delay(50);
            return _mockGames.Where(g => g.HasJackpot).Take(count).ToList();
        }

        public async Task<List<Game>> GetRelatedGamesAsync(int gameId, int count = 5)
        {
            await Task.Delay(50);
            var game = _mockGames.FirstOrDefault(g => g.Id == gameId);
            if (game == null) return new List<Game>();

            return _mockGames
                .Where(g => g.Id != gameId && g.Category == game.Category)
                .Take(count)
                .ToList();
        }

        public async Task<List<string>> GetProvidersAsync()
        {
            await Task.Delay(50);
            return _mockGames.Select(g => g.Provider).Distinct().OrderBy(p => p).ToList();
        }

        public async Task<List<GameCategory>> GetCategoriesAsync()
        {
            await Task.Delay(50);
            return _mockGames.Select(g => g.Category).Distinct().OrderBy(c => c).ToList();
        }
    }
}