using Microsoft.AspNetCore.Identity;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Player"))
            {
                await roleManager.CreateAsync(new IdentityRole("Player"));
            }

            // Seed Admin User
            if (!context.Users.Any(u => u.Email == "admin@safecasino.be"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@safecasino.be",
                    Email = "admin@safecasino.be",
                    FirstName = "Admin",
                    LastName = "SafeCasino",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RegistrationDate = DateTime.Now,
                    Balance = 10000m,
                    IsVerified = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Game Providers
            if (!context.GameProviders.Any())
            {
                var providers = new List<GameProvider>
                {
                    new GameProvider
                    {
                        Name = "Evolution Gaming",
                        Description = "De wereldleider in live casino oplossingen",
                        LogoUrl = "/images/providers/evolution.png",
                        IsActive = true
                    },
                    new GameProvider
                    {
                        Name = "NetEnt",
                        Description = "Premium gaming provider met innovatieve slots",
                        LogoUrl = "/images/providers/netent.png",
                        IsActive = true
                    },
                    new GameProvider
                    {
                        Name = "Pragmatic Play",
                        Description = "Toonaangevende content provider in de iGaming industrie",
                        LogoUrl = "/images/providers/pragmatic.png",
                        IsActive = true
                    },
                    new GameProvider
                    {
                        Name = "Play'n GO",
                        Description = "Bekroonde casino content leverancier",
                        LogoUrl = "/images/providers/playngo.png",
                        IsActive = true
                    },
                    new GameProvider
                    {
                        Name = "Microgaming",
                        Description = "Een pionier in online gaming software",
                        LogoUrl = "/images/providers/microgaming.png",
                        IsActive = true
                    }
                };

                await context.GameProviders.AddRangeAsync(providers);
                await context.SaveChangesAsync();
            }

            // Seed Game Categories
            if (!context.GameCategories.Any())
            {
                var categories = new List<GameCategory>
                {
                    new GameCategory
                    {
                        Name = "Blackjack",
                        Description = "Klassieke en moderne blackjack varianten",
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new GameCategory
                    {
                        Name = "Live Casino",
                        Description = "Live dealer games met echte dealers",
                        DisplayOrder = 2,
                        IsActive = true
                    },
                    new GameCategory
                    {
                        Name = "Roulette",
                        Description = "Europese, Amerikaanse en Franse roulette",
                        DisplayOrder = 3,
                        IsActive = true
                    },
                    new GameCategory
                    {
                        Name = "Poker",
                        Description = "Video poker en table poker games",
                        DisplayOrder = 4,
                        IsActive = true
                    },
                    new GameCategory
                    {
                        Name = "Slots",
                        Description = "Duizenden slot machines met verschillende thema's",
                        DisplayOrder = 5,
                        IsActive = true
                    }
                };

                await context.GameCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed 25 Casino Games met correcte image paths
            if (!context.CasinoGames.Any())
            {
                var providers = context.GameProviders.ToList();
                var categories = context.GameCategories.ToList();

                var blackjackCategory = categories.First(c => c.Name == "Blackjack");
                var rouletteCategory = categories.First(c => c.Name == "Roulette");
                var slotsCategory = categories.First(c => c.Name == "Slots");
                var pokerCategory = categories.First(c => c.Name == "Poker");
                var liveCategory = categories.First(c => c.Name == "Live Casino");

                var games = new List<CasinoGame>
                {
                    // ========== BLACKJACK GAMES (5) ==========
                    new CasinoGame
                    {
                        Name = "Classic Blackjack",
                        Description = "Het traditionele blackjack spel met optimale RTP",
                        ThumbnailUrl = "/images/games/blackjack-classic.jpg",
                        CategoryId = blackjackCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 99.5m,
                        MinimumBet = 1m,
                        MaximumBet = 1000m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 15420,
                        CreatedDate = DateTime.Now.AddMonths(-6)
                    },
                    new CasinoGame
                    {
                        Name = "Premium Blackjack",
                        Description = "VIP blackjack met hogere inzet limieten",
                        ThumbnailUrl = "/images/games/blackjack-premium.jpg",
                        CategoryId = blackjackCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 99.6m,
                        MinimumBet = 10m,
                        MaximumBet = 5000m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 8930,
                        CreatedDate = DateTime.Now.AddMonths(-4)
                    },
                    new CasinoGame
                    {
                        Name = "Multihand Blackjack",
                        Description = "Speel tot 5 handen tegelijk",
                        ThumbnailUrl = "/images/games/blackjack-multihand.jpg",
                        CategoryId = blackjackCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 99.4m,
                        MinimumBet = 2m,
                        MaximumBet = 2000m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 5670,
                        CreatedDate = DateTime.Now.AddMonths(-8)
                    },
                    new CasinoGame
                    {
                        Name = "VIP Blackjack",
                        Description = "Exclusief blackjack voor high rollers",
                        ThumbnailUrl = "/images/games/blackjack-vip.jpg",
                        CategoryId = blackjackCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 99.7m,
                        MinimumBet = 50m,
                        MaximumBet = 10000m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 2340,
                        CreatedDate = DateTime.Now.AddMonths(-3)
                    },
                    new CasinoGame
                    {
                        Name = "Blackjack Switch",
                        Description = "Innovatieve variant waarbij je kaarten kunt wisselen",
                        ThumbnailUrl = "/images/games/blackjack-switch.jpg",
                        CategoryId = blackjackCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 99.2m,
                        MinimumBet = 1m,
                        MaximumBet = 500m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 1890,
                        CreatedDate = DateTime.Now.AddDays(-15)
                    },

                    // ========== ROULETTE GAMES (5) ==========
                    new CasinoGame
                    {
                        Name = "European Roulette",
                        Description = "Klassieke Europese roulette met 37 nummers",
                        ThumbnailUrl = "/images/games/roulette-european.jpg",
                        CategoryId = rouletteCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 97.3m,
                        MinimumBet = 0.5m,
                        MaximumBet = 1000m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 18750,
                        CreatedDate = DateTime.Now.AddMonths(-10)
                    },
                    new CasinoGame
                    {
                        Name = "American Roulette",
                        Description = "Amerikaanse variant met dubbel nul",
                        ThumbnailUrl = "/images/games/roulette-american.jpg",
                        CategoryId = rouletteCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 94.7m,
                        MinimumBet = 0.5m,
                        MaximumBet = 800m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 7890,
                        CreatedDate = DateTime.Now.AddMonths(-9)
                    },
                    new CasinoGame
                    {
                        Name = "French Roulette",
                        Description = "Franse roulette met La Partage regel",
                        ThumbnailUrl = "/images/games/roulette-french.jpg",
                        CategoryId = rouletteCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 98.65m,
                        MinimumBet = 1m,
                        MaximumBet = 1500m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 12340,
                        CreatedDate = DateTime.Now.AddMonths(-7)
                    },
                    new CasinoGame
                    {
                        Name = "VIP Roulette",
                        Description = "Premium roulette voor high rollers",
                        ThumbnailUrl = "/images/games/roulette-vip.jpg",
                        CategoryId = rouletteCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 97.3m,
                        MinimumBet = 25m,
                        MaximumBet = 5000m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 3450,
                        CreatedDate = DateTime.Now.AddMonths(-5)
                    },
                    new CasinoGame
                    {
                        Name = "Lightning Roulette",
                        Description = "Electrifying roulette met random multipliers tot 500x",
                        ThumbnailUrl = "/images/games/roulette-lightning.jpg",
                        CategoryId = rouletteCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 97.1m,
                        MinimumBet = 0.2m,
                        MaximumBet = 2000m,
                        IsPopular = true,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 9870,
                        CreatedDate = DateTime.Now.AddDays(-30)
                    },

                    // ========== SLOTS GAMES (10) ==========
                    new CasinoGame
                    {
                        Name = "Starburst",
                        Description = "Iconische NetEnt slot met expanding wilds",
                        ThumbnailUrl = "/images/games/slots-starburst.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 96.1m,
                        MinimumBet = 0.1m,
                        MaximumBet = 100m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 45780,
                        CreatedDate = DateTime.Now.AddYears(-2)
                    },
                    new CasinoGame
                    {
                        Name = "Megaways Madness",
                        Description = "Tot 117,649 winlijnen met cascading reels",
                        ThumbnailUrl = "/images/games/slots-megaways.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 96.5m,
                        MinimumBet = 0.2m,
                        MaximumBet = 50m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 34560,
                        CreatedDate = DateTime.Now.AddMonths(-14)
                    },
                    new CasinoGame
                    {
                        Name = "Gonzo's Quest",
                        Description = "Avontuurlijke slot met avalanche feature",
                        ThumbnailUrl = "/images/games/slots-gonzo.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 96.0m,
                        MinimumBet = 0.2m,
                        MaximumBet = 50m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 38920,
                        CreatedDate = DateTime.Now.AddYears(-1)
                    },
                    new CasinoGame
                    {
                        Name = "Bonanza",
                        Description = "Megaways slot met gratis spins feature",
                        ThumbnailUrl = "/images/games/slots-bonanza.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 96.0m,
                        MinimumBet = 0.2m,
                        MaximumBet = 40m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 23450,
                        CreatedDate = DateTime.Now.AddMonths(-11)
                    },
                    new CasinoGame
                    {
                        Name = "Book of Dead",
                        Description = "Egyptische avontuur slot met expanding symbols",
                        ThumbnailUrl = "/images/games/slots-book-of-dead.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[3].Id,
                        RtpPercentage = 96.2m,
                        MinimumBet = 0.1m,
                        MaximumBet = 100m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 41230,
                        CreatedDate = DateTime.Now.AddMonths(-18)
                    },
                    new CasinoGame
                    {
                        Name = "Wild Safari",
                        Description = "Safari thema slot met wilde dieren en multipliers",
                        ThumbnailUrl = "/images/games/slots-wild-safari.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 96.3m,
                        MinimumBet = 0.2m,
                        MaximumBet = 60m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 5670,
                        CreatedDate = DateTime.Now.AddDays(-20)
                    },
                    new CasinoGame
                    {
                        Name = "Dragon's Fortune",
                        Description = "Aziatische draak slot met jackpot feature",
                        ThumbnailUrl = "/images/games/slots-dragon-fortune.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 96.4m,
                        MinimumBet = 0.3m,
                        MaximumBet = 75m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 4890,
                        CreatedDate = DateTime.Now.AddDays(-18)
                    },
                    new CasinoGame
                    {
                        Name = "Ocean Treasures",
                        Description = "Onderwaterwereld slot met free spins",
                        ThumbnailUrl = "/images/games/slots-ocean-treasures.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[3].Id,
                        RtpPercentage = 96.1m,
                        MinimumBet = 0.2m,
                        MaximumBet = 50m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 3450,
                        CreatedDate = DateTime.Now.AddDays(-25)
                    },
                    new CasinoGame
                    {
                        Name = "Eagle's Flight",
                        Description = "Vogelthema slot met soaring wins feature",
                        ThumbnailUrl = "/images/games/slots-eagle-flight.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 96.2m,
                        MinimumBet = 0.2m,
                        MaximumBet = 60m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 2890,
                        CreatedDate = DateTime.Now.AddDays(-12)
                    },
                    new CasinoGame
                    {
                        Name = "Lucky Lions",
                        Description = "Leeuwen thema slot met stacked wilds",
                        ThumbnailUrl = "/images/games/slots-lucky-lions.jpg",
                        CategoryId = slotsCategory.Id,
                        ProviderId = providers[3].Id,
                        RtpPercentage = 96.0m,
                        MinimumBet = 0.15m,
                        MaximumBet = 45m,
                        IsPopular = false,
                        IsNew = true,
                        IsAvailable = true,
                        PlayCount = 4120,
                        CreatedDate = DateTime.Now.AddDays(-8)
                    },

                    // ========== POKER GAMES (3) ==========
                    new CasinoGame
                    {
                        Name = "Texas Hold'em Poker",
                        Description = "De populairste poker variant wereldwijd",
                        ThumbnailUrl = "/images/games/poker-texas-holdem.jpg",
                        CategoryId = pokerCategory.Id,
                        ProviderId = providers[2].Id,
                        RtpPercentage = 99.5m,
                        MinimumBet = 2m,
                        MaximumBet = 500m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 12450,
                        CreatedDate = DateTime.Now.AddMonths(-12)
                    },
                    new CasinoGame
                    {
                        Name = "Caribbean Stud Poker",
                        Description = "Poker tegen de dealer met progressieve jackpot",
                        ThumbnailUrl = "/images/games/poker-caribbean-stud.jpg",
                        CategoryId = pokerCategory.Id,
                        ProviderId = providers[1].Id,
                        RtpPercentage = 97.5m,
                        MinimumBet = 1m,
                        MaximumBet = 200m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 6780,
                        CreatedDate = DateTime.Now.AddMonths(-8)
                    },
                    new CasinoGame
                    {
                        Name = "Three Card Poker",
                        Description = "Snelle poker variant met 3 kaarten",
                        ThumbnailUrl = "/images/games/poker-three-card.jpg",
                        CategoryId = pokerCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 98.0m,
                        MinimumBet = 1m,
                        MaximumBet = 300m,
                        IsPopular = false,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 5340,
                        CreatedDate = DateTime.Now.AddMonths(-6)
                    },

                    // ========== LIVE CASINO GAMES (2) ==========
                    new CasinoGame
                    {
                        Name = "Live Casino Roulette",
                        Description = "Live roulette met professionele dealers",
                        ThumbnailUrl = "/images/games/live-casino-roulette.jpg",
                        CategoryId = liveCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 97.3m,
                        MinimumBet = 1m,
                        MaximumBet = 2000m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 23450,
                        CreatedDate = DateTime.Now.AddMonths(-15)
                    },
                    new CasinoGame
                    {
                        Name = "Live Casino Blackjack",
                        Description = "Live blackjack aan echte tafels",
                        ThumbnailUrl = "/images/games/live-casino-blackjack.jpg",
                        CategoryId = liveCategory.Id,
                        ProviderId = providers[0].Id,
                        RtpPercentage = 99.5m,
                        MinimumBet = 5m,
                        MaximumBet = 3000m,
                        IsPopular = true,
                        IsNew = false,
                        IsAvailable = true,
                        PlayCount = 19870,
                        CreatedDate = DateTime.Now.AddMonths(-13)
                    }
                };

                await context.CasinoGames.AddRangeAsync(games);
                await context.SaveChangesAsync();
            }
        }
    }
}