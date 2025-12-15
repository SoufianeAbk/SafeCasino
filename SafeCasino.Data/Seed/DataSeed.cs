using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Seed
{
    /// <summary>
    /// Seed data voor game categorieën, providers en games
    /// </summary>
    public static class DataSeed
    {
        /// <summary>
        /// Seed alle game gerelateerde data
        /// </summary>
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedGameCategories(modelBuilder);
            SeedGameProviders(modelBuilder);
            SeedCasinoGames(modelBuilder);
        }

        /// <summary>
        /// Seed de game categorieën
        /// </summary>
        private static void SeedGameCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCategory>().HasData(
                new GameCategory
                {
                    Id = 1,
                    Name = "Blackjack",
                    Description = "Klassieke kaartspellen waarbij je probeert zo dicht mogelijk bij 21 te komen",
                    IconUrl = "/images/categories/blackjack.svg",
                    IsActive = true,
                    DisplayOrder = 1
                },
                new GameCategory
                {
                    Id = 2,
                    Name = "Live Casino",
                    Description = "Speel live met echte dealers via videoverbinding",
                    IconUrl = "/images/categories/live-casino.svg",
                    IsActive = true,
                    DisplayOrder = 2
                },
                new GameCategory
                {
                    Id = 3,
                    Name = "Roulette",
                    Description = "Draai aan het rad en voorspel waar de bal zal landen",
                    IconUrl = "/images/categories/roulette.svg",
                    IsActive = true,
                    DisplayOrder = 3
                },
                new GameCategory
                {
                    Id = 4,
                    Name = "Poker",
                    Description = "Test je pokerskills tegen andere spelers",
                    IconUrl = "/images/categories/poker.svg",
                    IsActive = true,
                    DisplayOrder = 4
                },
                new GameCategory
                {
                    Id = 5,
                    Name = "Slots",
                    Description = "Gokkasten met verschillende thema's en jackpots",
                    IconUrl = "/images/categories/slots.svg",
                    IsActive = true,
                    DisplayOrder = 5
                }
            );
        }

        /// <summary>
        /// Seed de game providers
        /// </summary>
        private static void SeedGameProviders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameProvider>().HasData(
                new GameProvider
                {
                    Id = 1,
                    Name = "NetEnt",
                    Description = "Toonaangevende provider van premium gaming oplossingen",
                    WebsiteUrl = "https://www.netent.com",
                    LogoUrl = "/images/providers/netent.png",
                    IsActive = true
                },
                new GameProvider
                {
                    Id = 2,
                    Name = "Evolution Gaming",
                    Description = "Specialist in live casino games",
                    WebsiteUrl = "https://www.evolution.com",
                    LogoUrl = "/images/providers/evolution.png",
                    IsActive = true
                },
                new GameProvider
                {
                    Id = 3,
                    Name = "Microgaming",
                    Description = "Een van de oudste en meest gerespecteerde game developers",
                    WebsiteUrl = "https://www.microgaming.co.uk",
                    LogoUrl = "/images/providers/microgaming.png",
                    IsActive = true
                },
                new GameProvider
                {
                    Id = 4,
                    Name = "Play'n GO",
                    Description = "Innovatieve mobile-first game developer",
                    WebsiteUrl = "https://www.playngo.com",
                    LogoUrl = "/images/providers/playngo.png",
                    IsActive = true
                },
                new GameProvider
                {
                    Id = 5,
                    Name = "Pragmatic Play",
                    Description = "Multi-product content provider voor de gaming industrie",
                    WebsiteUrl = "https://www.pragmaticplay.com",
                    LogoUrl = "/images/providers/pragmatic.png",
                    IsActive = true
                }
            );
        }

        /// <summary>
        /// Seed de casino games (5 per categorie)
        /// </summary>
        private static void SeedCasinoGames(ModelBuilder modelBuilder)
        {
            var games = new List<CasinoGame>();
            int gameId = 1;

            // Blackjack Games (Categorie 1)
            games.AddRange(new[]
            {
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Classic Blackjack",
                    Description = "Traditionele blackjack met standaard regels en uitbetalingen",
                    ThumbnailUrl = "/images/games/classic-blackjack.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 1000m,
                    RtpPercentage = 99.41m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-6),
                    PlayCount = 15420,
                    CategoryId = 1,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "European Blackjack",
                    Description = "Europese variant met hole card regel",
                    ThumbnailUrl = "/images/games/european-blackjack.jpg",
                    MinimumBet = 5m,
                    MaximumBet = 2000m,
                    RtpPercentage = 99.60m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-8),
                    PlayCount = 12350,
                    CategoryId = 1,
                    ProviderId = 2
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Atlantic City Blackjack",
                    Description = "Amerikaanse blackjack variant met late surrender optie",
                    ThumbnailUrl = "/images/games/atlantic-blackjack.jpg",
                    MinimumBet = 2m,
                    MaximumBet = 1500m,
                    RtpPercentage = 99.65m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddMonths(-10),
                    PlayCount = 8920,
                    CategoryId = 1,
                    ProviderId = 3
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Perfect Pairs Blackjack",
                    Description = "Blackjack met extra side bet op pairs",
                    ThumbnailUrl = "/images/games/perfect-pairs-blackjack.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 500m,
                    RtpPercentage = 98.87m,
                    IsAvailable = true,
                    IsNew = true,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddDays(-15),
                    PlayCount = 3450,
                    CategoryId = 1,
                    ProviderId = 4
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Double Exposure Blackjack",
                    Description = "Beide dealer kaarten zijn zichtbaar",
                    ThumbnailUrl = "/images/games/double-exposure-blackjack.jpg",
                    MinimumBet = 5m,
                    MaximumBet = 1000m,
                    RtpPercentage = 99.33m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddMonths(-4),
                    PlayCount = 5670,
                    CategoryId = 1,
                    ProviderId = 5
                }
            });

            // Live Casino Games (Categorie 2)
            games.AddRange(new[]
            {
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Live Blackjack VIP",
                    Description = "Exclusieve live blackjack tafel met professionele dealers",
                    ThumbnailUrl = "/images/games/live-blackjack-vip.jpg",
                    MinimumBet = 50m,
                    MaximumBet = 10000m,
                    RtpPercentage = 99.28m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-12),
                    PlayCount = 23450,
                    CategoryId = 2,
                    ProviderId = 2
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Live Roulette",
                    Description = "Authentieke roulette ervaring met live dealer",
                    ThumbnailUrl = "/images/games/live-roulette.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 5000m,
                    RtpPercentage = 97.30m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-15),
                    PlayCount = 34560,
                    CategoryId = 2,
                    ProviderId = 2
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Live Baccarat",
                    Description = "Klassiek baccarat spel met live dealers",
                    ThumbnailUrl = "/images/games/live-baccarat.jpg",
                    MinimumBet = 10m,
                    MaximumBet = 15000m,
                    RtpPercentage = 98.94m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-9),
                    PlayCount = 19870,
                    CategoryId = 2,
                    ProviderId = 2
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Live Casino Hold'em",
                    Description = "Poker variant tegen de dealer in live setting",
                    ThumbnailUrl = "/images/games/live-casino-holdem.jpg",
                    MinimumBet = 5m,
                    MaximumBet = 3000m,
                    RtpPercentage = 97.84m,
                    IsAvailable = true,
                    IsNew = true,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddDays(-30),
                    PlayCount = 4560,
                    CategoryId = 2,
                    ProviderId = 2
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Live Dream Catcher",
                    Description = "Money wheel spel met live presentator",
                    ThumbnailUrl = "/images/games/live-dream-catcher.jpg",
                    MinimumBet = 0.10m,
                    MaximumBet = 2000m,
                    RtpPercentage = 95.80m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-7),
                    PlayCount = 28930,
                    CategoryId = 2,
                    ProviderId = 2
                }
            });

            // Roulette Games (Categorie 3)
            games.AddRange(new[]
            {
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "European Roulette",
                    Description = "Klassieke Europese roulette met single zero",
                    ThumbnailUrl = "/images/games/european-roulette.jpg",
                    MinimumBet = 0.10m,
                    MaximumBet = 1000m,
                    RtpPercentage = 97.30m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-18),
                    PlayCount = 45620,
                    CategoryId = 3,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "French Roulette",
                    Description = "Franse variant met La Partage regel",
                    ThumbnailUrl = "/images/games/french-roulette.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 2000m,
                    RtpPercentage = 98.65m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-14),
                    PlayCount = 32450,
                    CategoryId = 3,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "American Roulette",
                    Description = "Amerikaanse variant met double zero",
                    ThumbnailUrl = "/images/games/american-roulette.jpg",
                    MinimumBet = 0.50m,
                    MaximumBet = 1500m,
                    RtpPercentage = 94.74m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddMonths(-20),
                    PlayCount = 18760,
                    CategoryId = 3,
                    ProviderId = 3
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Speed Roulette",
                    Description = "Snellere roulette variant met kortere rondes",
                    ThumbnailUrl = "/images/games/speed-roulette.jpg",
                    MinimumBet = 0.20m,
                    MaximumBet = 500m,
                    RtpPercentage = 97.30m,
                    IsAvailable = true,
                    IsNew = true,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddDays(-45),
                    PlayCount = 12340,
                    CategoryId = 3,
                    ProviderId = 4
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Multi-Wheel Roulette",
                    Description = "Speel op meerdere wielen tegelijk",
                    ThumbnailUrl = "/images/games/multi-wheel-roulette.jpg",
                    MinimumBet = 0.10m,
                    MaximumBet = 100m,
                    RtpPercentage = 97.30m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddMonths(-5),
                    PlayCount = 8920,
                    CategoryId = 3,
                    ProviderId = 5
                }
            });

            // Poker Games (Categorie 4)
            games.AddRange(new[]
            {
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Texas Hold'em Poker",
                    Description = "Meest populaire poker variant wereldwijd",
                    ThumbnailUrl = "/images/games/texas-holdem.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 5000m,
                    RtpPercentage = 98.00m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-24),
                    PlayCount = 56780,
                    CategoryId = 4,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Caribbean Stud Poker",
                    Description = "Tropische poker variant met progressieve jackpot",
                    ThumbnailUrl = "/images/games/caribbean-stud.jpg",
                    MinimumBet = 2m,
                    MaximumBet = 2000m,
                    RtpPercentage = 97.48m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddMonths(-16),
                    PlayCount = 14560,
                    CategoryId = 4,
                    ProviderId = 3
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Three Card Poker",
                    Description = "Snelle poker variant met drie kaarten",
                    ThumbnailUrl = "/images/games/three-card-poker.jpg",
                    MinimumBet = 1m,
                    MaximumBet = 1000m,
                    RtpPercentage = 96.63m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-11),
                    PlayCount = 23450,
                    CategoryId = 4,
                    ProviderId = 4
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Pai Gow Poker",
                    Description = "Chinese poker variant met twee handen",
                    ThumbnailUrl = "/images/games/pai-gow-poker.jpg",
                    MinimumBet = 5m,
                    MaximumBet = 3000m,
                    RtpPercentage = 97.27m,
                    IsAvailable = true,
                    IsNew = true,
                    IsPopular = false,
                    CreatedDate = DateTime.Now.AddDays(-20),
                    PlayCount = 3890,
                    CategoryId = 4,
                    ProviderId = 5
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Video Poker Jacks or Better",
                    Description = "Klassieke video poker met standaard uitbetalingstabel",
                    ThumbnailUrl = "/images/games/video-poker-jacks.jpg",
                    MinimumBet = 0.25m,
                    MaximumBet = 100m,
                    RtpPercentage = 99.54m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-22),
                    PlayCount = 34670,
                    CategoryId = 4,
                    ProviderId = 1
                }
            });

            // Slots Games (Categorie 5)
            games.AddRange(new[]
            {
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Starburst",
                    Description = "Iconische slot met expanding wilds en re-spins",
                    ThumbnailUrl = "/images/games/starburst.jpg",
                    MinimumBet = 0.10m,
                    MaximumBet = 100m,
                    RtpPercentage = 96.09m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-36),
                    PlayCount = 123450,
                    CategoryId = 5,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Book of Dead",
                    Description = "Egyptische avonturen met expanding symbols",
                    ThumbnailUrl = "/images/games/book-of-dead.jpg",
                    MinimumBet = 0.01m,
                    MaximumBet = 100m,
                    RtpPercentage = 96.21m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-28),
                    PlayCount = 98760,
                    CategoryId = 5,
                    ProviderId = 4
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Mega Moolah",
                    Description = "Progressieve jackpot slot met safari thema",
                    ThumbnailUrl = "/images/games/mega-moolah.jpg",
                    MinimumBet = 0.25m,
                    MaximumBet = 6.25m,
                    RtpPercentage = 88.12m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-48),
                    PlayCount = 145670,
                    CategoryId = 5,
                    ProviderId = 3
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Gonzo's Quest",
                    Description = "Avonturenslot met avalanche feature",
                    ThumbnailUrl = "/images/games/gonzos-quest.jpg",
                    MinimumBet = 0.20m,
                    MaximumBet = 50m,
                    RtpPercentage = 95.97m,
                    IsAvailable = true,
                    IsNew = false,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddMonths(-40),
                    PlayCount = 87650,
                    CategoryId = 5,
                    ProviderId = 1
                },
                new CasinoGame
                {
                    Id = gameId++,
                    Name = "Sweet Bonanza",
                    Description = "Kleurrijke fruitmachine met tumble feature",
                    ThumbnailUrl = "/images/games/sweet-bonanza.jpg",
                    MinimumBet = 0.20m,
                    MaximumBet = 100m,
                    RtpPercentage = 96.51m,
                    IsAvailable = true,
                    IsNew = true,
                    IsPopular = true,
                    CreatedDate = DateTime.Now.AddDays(-60),
                    PlayCount = 45890,
                    CategoryId = 5,
                    ProviderId = 5
                }
            });

            modelBuilder.Entity<CasinoGame>().HasData(games);
        }
    }
}