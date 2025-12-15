using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeCasino.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CasinoGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MinimumBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RtpPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsNew = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasinoGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasinoGames_GameCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GameCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CasinoGames_GameProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "GameProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_CasinoGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CasinoGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameCategories",
                columns: new[] { "Id", "Description", "DisplayOrder", "IconUrl", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Klassieke kaartspellen waarbij je probeert zo dicht mogelijk bij 21 te komen", 1, "/images/categories/blackjack.svg", true, "Blackjack" },
                    { 2, "Speel live met echte dealers via videoverbinding", 2, "/images/categories/live-casino.svg", true, "Live Casino" },
                    { 3, "Draai aan het rad en voorspel waar de bal zal landen", 3, "/images/categories/roulette.svg", true, "Roulette" },
                    { 4, "Test je pokerskills tegen andere spelers", 4, "/images/categories/poker.svg", true, "Poker" },
                    { 5, "Gokkasten met verschillende thema's en jackpots", 5, "/images/categories/slots.svg", true, "Slots" }
                });

            migrationBuilder.InsertData(
                table: "GameProviders",
                columns: new[] { "Id", "Description", "IsActive", "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "Toonaangevende provider van premium gaming oplossingen", true, "/images/providers/netent.png", "NetEnt", "https://www.netent.com" },
                    { 2, "Specialist in live casino games", true, "/images/providers/evolution.png", "Evolution Gaming", "https://www.evolution.com" },
                    { 3, "Een van de oudste en meest gerespecteerde game developers", true, "/images/providers/microgaming.png", "Microgaming", "https://www.microgaming.co.uk" },
                    { 4, "Innovatieve mobile-first game developer", true, "/images/providers/playngo.png", "Play'n GO", "https://www.playngo.com" },
                    { 5, "Multi-product content provider voor de gaming industrie", true, "/images/providers/pragmatic.png", "Pragmatic Play", "https://www.pragmaticplay.com" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "c648b165-0da4-4894-be8a-23077e93c5e0", "Administrator met volledige toegang tot het systeem", "Admin", "ADMIN" },
                    { "2", "858dc53e-d95f-4ec6-8eed-f71ad84e763e", "Moderator met beperkte beheersrechten", "Moderator", "MODERATOR" },
                    { "3", "fda850d9-600d-4545-b69f-4ba9c6555709", "Standaard speler rol", "Player", "PLAYER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Balance", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsVerified", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "admin-user-id", 0, 10000m, "33b306ad-acb2-4145-b105-7d40b58b0dfb", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@safecasino.be", true, "Admin", true, "Casino", false, null, "ADMIN@SAFECASINO.BE", "ADMIN@SAFECASINO.BE", "AQAAAAIAAYagAAAAEF1LyMOfCjJ+fW38FumwrxydRkWP7ejWRlHy6E0EZ30YndiH7/cMujke7f2brTodWw==", null, false, new DateTime(2025, 12, 15, 20, 31, 59, 520, DateTimeKind.Local).AddTicks(6000), "f1869859-576f-4dbc-85a7-6c6773d77ed7", false, "admin@safecasino.be" },
                    { "player-user-id", 0, 500m, "d0825400-d5b7-49d0-acc4-834c74e470e5", new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "speler@safecasino.be", true, "Jan", true, "Janssens", false, null, "SPELER@SAFECASINO.BE", "SPELER@SAFECASINO.BE", "AQAAAAIAAYagAAAAECjVTHA31B0JYBmVXY5KSsRjafIVOD2ebudusBK3R4K0GSQ/3QLMIW5vfvZIE8iW+A==", null, false, new DateTime(2025, 12, 15, 20, 31, 59, 671, DateTimeKind.Local).AddTicks(2787), "d71db9e9-908d-4135-8ce3-d755e57c16ee", false, "speler@safecasino.be" }
                });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(141), "Traditionele blackjack met standaard regels en uitbetalingen", true, true, 1000m, 1m, "Classic Blackjack", 15420, 1, 99.41m, "/images/games/classic-blackjack.jpg" },
                    { 2, 1, new DateTime(2025, 4, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(1510), "Europese variant met hole card regel", true, true, 2000m, 5m, "European Blackjack", 12350, 2, 99.60m, "/images/games/european-blackjack.jpg" }
                });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 3, 1, new DateTime(2025, 2, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(1576), "Amerikaanse blackjack variant met late surrender optie", true, 1500m, 2m, "Atlantic City Blackjack", 8920, 3, 99.65m, "/images/games/atlantic-blackjack.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsNew", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 4, 1, new DateTime(2025, 11, 30, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(1586), "Blackjack met extra side bet op pairs", true, true, 500m, 1m, "Perfect Pairs Blackjack", 3450, 4, 98.87m, "/images/games/perfect-pairs-blackjack.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 5, 1, new DateTime(2025, 8, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(1638), "Beide dealer kaarten zijn zichtbaar", true, 1000m, 5m, "Double Exposure Blackjack", 5670, 5, 99.33m, "/images/games/double-exposure-blackjack.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[,]
                {
                    { 6, 2, new DateTime(2024, 12, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3036), "Exclusieve live blackjack tafel met professionele dealers", true, true, 10000m, 50m, "Live Blackjack VIP", 23450, 2, 99.28m, "/images/games/live-blackjack-vip.jpg" },
                    { 7, 2, new DateTime(2024, 9, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3097), "Authentieke roulette ervaring met live dealer", true, true, 5000m, 1m, "Live Roulette", 34560, 2, 97.30m, "/images/games/live-roulette.jpg" },
                    { 8, 2, new DateTime(2025, 3, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3107), "Klassiek baccarat spel met live dealers", true, true, 15000m, 10m, "Live Baccarat", 19870, 2, 98.94m, "/images/games/live-baccarat.jpg" }
                });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsNew", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 9, 2, new DateTime(2025, 11, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3140), "Poker variant tegen de dealer in live setting", true, true, 3000m, 5m, "Live Casino Hold'em", 4560, 2, 97.84m, "/images/games/live-casino-holdem.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[,]
                {
                    { 10, 2, new DateTime(2025, 5, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3151), "Money wheel spel met live presentator", true, true, 2000m, 0.10m, "Live Dream Catcher", 28930, 2, 95.80m, "/images/games/live-dream-catcher.jpg" },
                    { 11, 3, new DateTime(2024, 6, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3181), "Klassieke Europese roulette met single zero", true, true, 1000m, 0.10m, "European Roulette", 45620, 1, 97.30m, "/images/games/european-roulette.jpg" },
                    { 12, 3, new DateTime(2024, 10, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3192), "Franse variant met La Partage regel", true, true, 2000m, 1m, "French Roulette", 32450, 1, 98.65m, "/images/games/french-roulette.jpg" }
                });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 13, 3, new DateTime(2024, 4, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3202), "Amerikaanse variant met double zero", true, 1500m, 0.50m, "American Roulette", 18760, 3, 94.74m, "/images/games/american-roulette.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsNew", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 14, 3, new DateTime(2025, 10, 31, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3211), "Snellere roulette variant met kortere rondes", true, true, true, 500m, 0.20m, "Speed Roulette", 12340, 4, 97.30m, "/images/games/speed-roulette.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 15, 3, new DateTime(2025, 7, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3221), "Speel op meerdere wielen tegelijk", true, 100m, 0.10m, "Multi-Wheel Roulette", 8920, 5, 97.30m, "/images/games/multi-wheel-roulette.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 16, 4, new DateTime(2023, 12, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3253), "Meest populaire poker variant wereldwijd", true, true, 5000m, 1m, "Texas Hold'em Poker", 56780, 1, 98.00m, "/images/games/texas-holdem.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 17, 4, new DateTime(2024, 8, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3264), "Tropische poker variant met progressieve jackpot", true, 2000m, 2m, "Caribbean Stud Poker", 14560, 3, 97.48m, "/images/games/caribbean-stud.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 18, 4, new DateTime(2025, 1, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3273), "Snelle poker variant met drie kaarten", true, true, 1000m, 1m, "Three Card Poker", 23450, 4, 96.63m, "/images/games/three-card-poker.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsNew", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 19, 4, new DateTime(2025, 11, 25, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3283), "Chinese poker variant met twee handen", true, true, 3000m, 5m, "Pai Gow Poker", 3890, 5, 97.27m, "/images/games/pai-gow-poker.jpg" });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[,]
                {
                    { 20, 4, new DateTime(2024, 2, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3293), "Klassieke video poker met standaard uitbetalingstabel", true, true, 100m, 0.25m, "Video Poker Jacks or Better", 34670, 1, 99.54m, "/images/games/video-poker-jacks.jpg" },
                    { 21, 5, new DateTime(2022, 12, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3305), "Iconische slot met expanding wilds en re-spins", true, true, 100m, 0.10m, "Starburst", 123450, 1, 96.09m, "/images/games/starburst.jpg" },
                    { 22, 5, new DateTime(2023, 8, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3315), "Egyptische avonturen met expanding symbols", true, true, 100m, 0.01m, "Book of Dead", 98760, 4, 96.21m, "/images/games/book-of-dead.jpg" },
                    { 23, 5, new DateTime(2021, 12, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3325), "Progressieve jackpot slot met safari thema", true, true, 6.25m, 0.25m, "Mega Moolah", 145670, 3, 88.12m, "/images/games/mega-moolah.jpg" },
                    { 24, 5, new DateTime(2022, 8, 15, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3335), "Avonturenslot met avalanche feature", true, true, 50m, 0.20m, "Gonzo's Quest", 87650, 1, 95.97m, "/images/games/gonzos-quest.jpg" }
                });

            migrationBuilder.InsertData(
                table: "CasinoGames",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "IsAvailable", "IsNew", "IsPopular", "MaximumBet", "MinimumBet", "Name", "PlayCount", "ProviderId", "RtpPercentage", "ThumbnailUrl" },
                values: new object[] { 25, 5, new DateTime(2025, 10, 16, 20, 31, 59, 818, DateTimeKind.Local).AddTicks(3344), "Kleurrijke fruitmachine met tumble feature", true, true, true, 100m, 0.20m, "Sweet Bonanza", 45890, 5, 96.51m, "/images/games/sweet-bonanza.jpg" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "admin-user-id" },
                    { "3", "player-user-id" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CasinoGames_CategoryId",
                table: "CasinoGames",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CasinoGames_IsNew",
                table: "CasinoGames",
                column: "IsNew");

            migrationBuilder.CreateIndex(
                name: "IX_CasinoGames_IsPopular",
                table: "CasinoGames",
                column: "IsPopular");

            migrationBuilder.CreateIndex(
                name: "IX_CasinoGames_Name",
                table: "CasinoGames",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CasinoGames_ProviderId",
                table: "CasinoGames",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCategories_DisplayOrder",
                table: "GameCategories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_GameCategories_Name",
                table: "GameCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameProviders_Name",
                table: "GameProviders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_GameId",
                table: "Reviews",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_IsApproved",
                table: "Reviews",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Rating",
                table: "Reviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "CasinoGames");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameCategories");

            migrationBuilder.DropTable(
                name: "GameProviders");
        }
    }
}
