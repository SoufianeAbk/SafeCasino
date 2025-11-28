using Microsoft.EntityFrameworkCore;
using SafeCasino.Data;
using SafeCasino.Models;

namespace SafeCasino.Helpers
{
    /// <summary>
    /// Helper class voor database initialization en seeding
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initialize the database with migrations and seed data
        /// </summary>
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SafeCasinoDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SafeCasinoDbContext>>();

            try
            {
                logger.LogInformation("Starting database initialization...");

                // Apply pending migrations
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations...");
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully.");
                }

                // Check if database can connect
                if (!await dbContext.Database.CanConnectAsync())
                {
                    logger.LogInformation("Creating database...");
                    await dbContext.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database created successfully.");
                }

                // Seed data if needed
                await SeedDataIfNeededAsync(dbContext, logger);

                logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error initializing database");
                throw;
            }
        }

        /// <summary>
        /// Seed initial data if database is empty
        /// </summary>
        private static async Task SeedDataIfNeededAsync(SafeCasinoDbContext dbContext, ILogger<SafeCasinoDbContext> logger)
        {
            try
            {
                // Check if users already exist
                var userCount = await dbContext.Users.CountAsync();
                if (userCount == 0)
                {
                    logger.LogInformation("Seeding initial users...");
                    await SeedUsersAsync(dbContext);
                }

                // Check if games already exist
                var gameCount = await dbContext.Games.CountAsync();
                if (gameCount == 0)
                {
                    logger.LogInformation("Seeding initial games...");
                    await SeedGamesAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding data");
                throw;
            }
        }

        /// <summary>
        /// Seed users into the database
        /// </summary>
        private static async Task SeedUsersAsync(SafeCasinoDbContext dbContext)
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "casinouser@ehb.be",
                    PasswordHash = "User!321",
                    UserType = "User",
                    CreatedDate = DateTime.UtcNow
                },
                new User
                {
                    Username = "casinoadmin@ehb.be",
                    PasswordHash = "Admin!321",
                    UserType = "Admin",
                    CreatedDate = DateTime.UtcNow
                }
            };

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Seed games into the database
        /// </summary>
        private static async Task SeedGamesAsync(SafeCasinoDbContext dbContext)
        {
            var games = GenerateGames();
            dbContext.Games.AddRange(games);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Generate seed game data
        /// </summary>
        private static List<Game> GenerateGames()
        {
            var games = new List<Game>();
            var providers = new[] { "NetEnt", "Microgaming", "PlayTech", "Evolution", "Pragmatic Play", "Novomatic" };

            var gamesByCategory = new Dictionary<GameCategory, string[]>
            {
                [GameCategory.Slots] = new[]
                {
                    "Book of Ra", "Starburst", "Gonzo's Quest", "Mega Moolah", "Dead or Alive"
                },
                [GameCategory.Roulette] = new[]
                {
                    "European Roulette", "American Roulette", "French Roulette", "Lightning Roulette",
                    "Immersive Roulette"
                },
                [GameCategory.Blackjack] = new[]
                {
                    "Classic Blackjack", "Blackjack Pro", "Perfect Blackjack", "VIP Blackjack",
                    "Blackjack Switch"
                },
                [GameCategory.LiveCasino] = new[]
                {
                    "Live Roulette", "Live Blackjack", "Live Baccarat", "Live Dream Catcher",
                    "Live Monopoly"
                },
                [GameCategory.Jackpot] = new[]
                {
                    "Mega Fortune", "Hall of Gods", "Arabian Nights", "Mega Moolah Isis",
                    "Major Millions"
                }
            };

            int id = 1;
            var random = new Random(42);

            foreach (var category in gamesByCategory)
            {
                var categoryFolder = category.Key.ToString().ToLower();

                for (int i = 0; i < category.Value.Length; i++)
                {
                    var name = category.Value[i];
                    var imageFilename = name.ToLower().Replace(" ", "-").Replace("'", "");

                    games.Add(new Game
                    {
                        Name = name,
                        Description = $"Speel {name} en ervaar de spanning!",
                        Provider = providers[random.Next(providers.Length)],
                        ThumbnailUrl = $"/images/games/{categoryFolder}/{imageFilename}.jpg",
                        GameUrl = $"/play/{id}",
                        Category = category.Key,
                        MinBet = category.Key == GameCategory.Slots ? 0.10m :
                                category.Key == GameCategory.Jackpot ? 0.20m : 1m,
                        MaxBet = category.Key == GameCategory.LiveCasino ? 1000m :
                                category.Key == GameCategory.Jackpot ? 200m : 500m,
                        RTP = 94m + (decimal)(random.NextDouble() * 4),
                        IsPopular = i < 2,
                        IsNew = i >= 3,
                        HasJackpot = category.Key == GameCategory.Jackpot,
                        AddedDate = DateTime.UtcNow.AddDays(-random.Next(365)),
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

                    id++;
                }
            }

            return games;
        }

        /// <summary>
        /// Clear all data from the database (use with caution!)
        /// </summary>
        public static async Task ClearDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SafeCasinoDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SafeCasinoDbContext>>();

            try
            {
                logger.LogWarning("CLEARING DATABASE - THIS CANNOT BE UNDONE!");

                dbContext.Games.RemoveRange(await dbContext.Games.ToListAsync());
                dbContext.Users.RemoveRange(await dbContext.Users.ToListAsync());

                await dbContext.SaveChangesAsync();

                logger.LogWarning("Database cleared successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error clearing database");
                throw;
            }
        }

        /// <summary>
        /// Reset database to initial state
        /// </summary>
        public static async Task ResetDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SafeCasinoDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SafeCasinoDbContext>>();

            try
            {
                logger.LogWarning("RESETTING DATABASE - ALL DATA WILL BE LOST!");

                // Delete database
                await dbContext.Database.EnsureDeletedAsync();
                logger.LogInformation("Database deleted.");

                // Recreate database
                await dbContext.Database.EnsureCreatedAsync();
                logger.LogInformation("Database recreated.");

                // Seed fresh data
                await SeedUsersAsync(dbContext);
                await SeedGamesAsync(dbContext);
                logger.LogInformation("Database reset with fresh data.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error resetting database");
                throw;
            }
        }
    }
}