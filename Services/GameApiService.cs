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

            // 5 categories with 10 games each = 50 games total
            var gamesByCategory = new Dictionary<GameCategory, string[]>
            {
                [GameCategory.Slots] = new[]
                {
                    "Book of Ra", "Starburst", "Gonzo's Quest", "Mega Moolah", "Dead or Alive",
                    "Bonanza", "Sweet Bonanza", "Gates of Olympus", "Wolf Gold", "Great Rhino"
                },
                [GameCategory.Roulette] = new[]
                {
                    "European Roulette", "American Roulette", "French Roulette", "Lightning Roulette",
                    "Immersive Roulette", "Speed Roulette", "Auto Roulette", "Double Ball Roulette",
                    "Mini Roulette", "Multi-Wheel Roulette"
                },
                [GameCategory.Blackjack] = new[]
                {
                    "Classic Blackjack", "Blackjack Pro", "Perfect Blackjack", "VIP Blackjack",
                    "Blackjack Switch", "Spanish 21", "Pontoon", "Double Exposure Blackjack",
                    "Atlantic City Blackjack", "Vegas Strip Blackjack"
                },
                [GameCategory.LiveCasino] = new[]
                {
                    "Live Roulette", "Live Blackjack", "Live Baccarat", "Live Dream Catcher",
                    "Live Monopoly", "Live Crazy Time", "Live Lightning Dice", "Live Deal or No Deal",
                    "Live Mega Ball", "Live Dragon Tiger"
                },
                [GameCategory.Jackpot] = new[]
                {
                    "Mega Fortune", "Hall of Gods", "Arabian Nights", "Mega Moolah Isis",
                    "Major Millions", "King Cashalot", "Treasure Nile", "Cash Splash",
                    "Fruit Fiesta", "SupaJax"
                }
            };

            int id = 1;

            foreach (var category in gamesByCategory)
            {
                for (int i = 0; i < category.Value.Length; i++)
                {
                    var name = category.Value[i];
                    games.Add(new Game
                    {
                        Id = id++,
                        Name = name,
                        Description = $"Speel {name} en ervaar de spanning!",
                        Provider = providers[Random.Shared.Next(providers.Length)],
                        ThumbnailUrl = $"https://picsum.photos/seed/{id}/400/300",
                        GameUrl = $"/play/{id}",
                        Category = category.Key,
                        MinBet = category.Key == GameCategory.Slots ? 0.10m :
                                category.Key == GameCategory.Jackpot ? 0.20m : 1m,
                        MaxBet = category.Key == GameCategory.LiveCasino ? 1000m :
                                category.Key == GameCategory.Jackpot ? 200m : 500m,
                        RTP = 94m + (decimal)(Random.Shared.NextDouble() * 4),
                        IsPopular = i < 3, // First 3 games in each category are popular
                        IsNew = i >= 7, // Last 3 games in each category are new
                        HasJackpot = category.Key == GameCategory.Jackpot,
                        AddedDate = DateTime.Now.AddDays(-Random.Shared.Next(365)),
                        Tags = category.Key switch
                        {
                            GameCategory.Slots => new List<string> { "Bonus", "Free Spins", "Wild" },
                            GameCategory.Roulette => new List<string> { "Klassiek", "Strategie" },
                            GameCategory.Blackjack => new List<string> { "Kaartspel", "Strategie" },
                            GameCategory.LiveCasino => new List<string> { "Live", "HD Stream", "Dealers" },
                            GameCategory.Jackpot => new List<string> { "Jackpot", "Grote Winsten", "Progressive" },
                            _ => new List<string>()
                        }
                    });
                }
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