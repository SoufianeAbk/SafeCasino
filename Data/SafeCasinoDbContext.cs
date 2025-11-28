using Microsoft.EntityFrameworkCore;
using SafeCasino.Models;

namespace SafeCasino.Data
{
    public class SafeCasinoDbContext : DbContext
    {
        public SafeCasinoDbContext(DbContextOptions<SafeCasinoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Game entity
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ThumbnailUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.GameUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.MinBet)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.MaxBet)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.RTP)
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Category)
                    .IsRequired();

                entity.Property(e => e.AddedDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed users
            var users = new User[]
            {
                new User
                {
                    Id = 1,
                    Username = "casinouser@ehb.be",
                    PasswordHash = "User!321",
                    UserType = "User",
                    CreatedDate = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    Username = "casinoadmin@ehb.be",
                    PasswordHash = "Admin!321",
                    UserType = "Admin",
                    CreatedDate = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            // Seed games
            var games = GenerateSeedGames();
            modelBuilder.Entity<Game>().HasData(games);
        }

        private List<Game> GenerateSeedGames()
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
            var random = new Random(42); // Fixed seed for consistent data

            foreach (var category in gamesByCategory)
            {
                var categoryFolder = category.Key.ToString().ToLower();

                for (int i = 0; i < category.Value.Length; i++)
                {
                    var name = category.Value[i];
                    var imageFilename = name.ToLower().Replace(" ", "-").Replace("'", "");

                    games.Add(new Game
                    {
                        Id = id++,
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
                }
            }

            return games;
        }
    }
}