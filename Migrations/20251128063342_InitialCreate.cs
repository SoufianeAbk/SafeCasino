using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeCasino.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GameUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    MinBet = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxBet = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RTP = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    HasJackpot = table.Column<bool>(type: "bit", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "AddedDate", "Category", "Description", "GameUrl", "HasJackpot", "IsNew", "IsPopular", "MaxBet", "MinBet", "Name", "Provider", "RTP", "ThumbnailUrl" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 14, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9177), 0, "Speel Book of Ra en ervaar de spanning!", "/play/2", false, false, true, 500m, 0.10m, "Book of Ra", "Pragmatic Play", 94.563629193493924m, "/images/games/slots/book-of-ra.jpg" },
                    { 2, new DateTime(2025, 8, 25, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9210), 0, "Speel Starburst en ervaar de spanning!", "/play/3", false, false, true, 500m, 0.10m, "Starburst", "Evolution", 94.673736896679614m, "/images/games/slots/starburst.jpg" },
                    { 3, new DateTime(2025, 9, 26, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9324), 0, "Speel Gonzo's Quest en ervaar de spanning!", "/play/4", false, false, false, 500m, 0.10m, "Gonzo's Quest", "Pragmatic Play", 96.05169116614931m, "/images/games/slots/gonzos-quest.jpg" },
                    { 4, new DateTime(2025, 8, 27, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9344), 0, "Speel Mega Moolah en ervaar de spanning!", "/play/5", false, true, false, 500m, 0.10m, "Mega Moolah", "Pragmatic Play", 94.938353833248072m, "/images/games/slots/mega-moolah.jpg" },
                    { 5, new DateTime(2025, 7, 12, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9361), 0, "Speel Dead or Alive en ervaar de spanning!", "/play/6", false, true, false, 500m, 0.10m, "Dead or Alive", "Evolution", 95.28093274928673m, "/images/games/slots/dead-or-alive.jpg" },
                    { 6, new DateTime(2025, 11, 16, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9389), 1, "Speel European Roulette en ervaar de spanning!", "/play/7", false, false, true, 500m, 1m, "European Roulette", "Microgaming", 96.06980439930679m, "/images/games/roulette/european-roulette.jpg" },
                    { 7, new DateTime(2025, 7, 6, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9408), 1, "Speel American Roulette en ervaar de spanning!", "/play/8", false, false, true, 500m, 1m, "American Roulette", "Pragmatic Play", 96.30878741587921m, "/images/games/roulette/american-roulette.jpg" },
                    { 8, new DateTime(2025, 3, 16, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9425), 1, "Speel French Roulette en ervaar de spanning!", "/play/9", false, false, false, 500m, 1m, "French Roulette", "NetEnt", 94.360450441185595m, "/images/games/roulette/french-roulette.jpg" },
                    { 9, new DateTime(2025, 11, 12, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9448), 1, "Speel Lightning Roulette en ervaar de spanning!", "/play/10", false, true, false, 500m, 1m, "Lightning Roulette", "Pragmatic Play", 96.15988232854748m, "/images/games/roulette/lightning-roulette.jpg" },
                    { 10, new DateTime(2025, 1, 2, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9468), 1, "Speel Immersive Roulette en ervaar de spanning!", "/play/11", false, true, false, 500m, 1m, "Immersive Roulette", "Pragmatic Play", 94.591045065126868m, "/images/games/roulette/immersive-roulette.jpg" },
                    { 11, new DateTime(2025, 9, 21, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9489), 2, "Speel Classic Blackjack en ervaar de spanning!", "/play/12", false, false, true, 500m, 1m, "Classic Blackjack", "Pragmatic Play", 96.06565485618341m, "/images/games/blackjack/classic-blackjack.jpg" },
                    { 12, new DateTime(2025, 4, 30, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9511), 2, "Speel Blackjack Pro en ervaar de spanning!", "/play/13", false, false, true, 500m, 1m, "Blackjack Pro", "NetEnt", 95.23279865143485m, "/images/games/blackjack/blackjack-pro.jpg" },
                    { 13, new DateTime(2025, 5, 17, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9529), 2, "Speel Perfect Blackjack en ervaar de spanning!", "/play/14", false, false, false, 500m, 1m, "Perfect Blackjack", "Pragmatic Play", 94.0158194433971399m, "/images/games/blackjack/perfect-blackjack.jpg" },
                    { 14, new DateTime(2025, 7, 11, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9547), 2, "Speel VIP Blackjack en ervaar de spanning!", "/play/15", false, true, false, 500m, 1m, "VIP Blackjack", "Pragmatic Play", 96.9934357083372m, "/images/games/blackjack/vip-blackjack.jpg" },
                    { 15, new DateTime(2025, 11, 10, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9631), 2, "Speel Blackjack Switch en ervaar de spanning!", "/play/16", false, true, false, 500m, 1m, "Blackjack Switch", "NetEnt", 94.105358056773133m, "/images/games/blackjack/blackjack-switch.jpg" },
                    { 16, new DateTime(2025, 10, 17, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9655), 3, "Speel Live Roulette en ervaar de spanning!", "/play/17", false, false, true, 1000m, 1m, "Live Roulette", "Pragmatic Play", 96.50582553749244m, "/images/games/livecasino/live-roulette.jpg" },
                    { 17, new DateTime(2025, 8, 15, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9674), 3, "Speel Live Blackjack en ervaar de spanning!", "/play/18", false, false, true, 1000m, 1m, "Live Blackjack", "Microgaming", 97.08971329736044m, "/images/games/livecasino/live-blackjack.jpg" },
                    { 18, new DateTime(2025, 11, 12, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9694), 3, "Speel Live Baccarat en ervaar de spanning!", "/play/19", false, false, false, 1000m, 1m, "Live Baccarat", "Evolution", 94.574585939093766m, "/images/games/livecasino/live-baccarat.jpg" },
                    { 19, new DateTime(2025, 11, 10, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9711), 3, "Speel Live Dream Catcher en ervaar de spanning!", "/play/20", false, true, false, 1000m, 1m, "Live Dream Catcher", "Evolution", 96.06365354268982m, "/images/games/livecasino/live-dream-catcher.jpg" },
                    { 20, new DateTime(2025, 4, 13, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9728), 3, "Speel Live Monopoly en ervaar de spanning!", "/play/21", false, true, false, 1000m, 1m, "Live Monopoly", "PlayTech", 96.82632930522148m, "/images/games/livecasino/live-monopoly.jpg" },
                    { 21, new DateTime(2025, 7, 18, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9749), 4, "Speel Mega Fortune en ervaar de spanning!", "/play/22", true, false, true, 200m, 0.20m, "Mega Fortune", "Microgaming", 94.0530150998630631m, "/images/games/jackpot/mega-fortune.jpg" },
                    { 22, new DateTime(2025, 3, 11, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9768), 4, "Speel Hall of Gods en ervaar de spanning!", "/play/23", true, false, true, 200m, 0.20m, "Hall of Gods", "Microgaming", 94.27464334306989m, "/images/games/jackpot/hall-of-gods.jpg" },
                    { 23, new DateTime(2025, 11, 24, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9785), 4, "Speel Arabian Nights en ervaar de spanning!", "/play/24", true, false, false, 200m, 0.20m, "Arabian Nights", "NetEnt", 95.85637431305664m, "/images/games/jackpot/arabian-nights.jpg" },
                    { 24, new DateTime(2025, 5, 25, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9802), 4, "Speel Mega Moolah Isis en ervaar de spanning!", "/play/25", true, true, false, 200m, 0.20m, "Mega Moolah Isis", "Pragmatic Play", 95.75671793229725m, "/images/games/jackpot/mega-moolah-isis.jpg" },
                    { 25, new DateTime(2025, 1, 30, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(9819), 4, "Speel Major Millions en ervaar de spanning!", "/play/26", true, true, false, 200m, 0.20m, "Major Millions", "PlayTech", 94.0358116366135942m, "/images/games/jackpot/major-millions.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "PasswordHash", "UserType", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 28, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(8441), "User!321", "User", "casinouser@ehb.be" },
                    { 2, new DateTime(2025, 11, 28, 6, 33, 41, 358, DateTimeKind.Utc).AddTicks(8445), "Admin!321", "Admin", "casinoadmin@ehb.be" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
