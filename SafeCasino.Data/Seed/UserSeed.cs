using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Seed
{
    /// <summary>
    /// Seed data voor gebruikers
    /// </summary>
    public static class UserSeed
    {
        /// <summary>
        /// Seed de gebruikers in de database
        /// </summary>
        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            // Admin gebruiker
            var adminUser = new ApplicationUser
            {
                Id = "admin-user-id",
                UserName = "admin@safecasino.be",
                NormalizedUserName = "ADMIN@SAFECASINO.BE",
                Email = "admin@safecasino.be",
                NormalizedEmail = "ADMIN@SAFECASINO.BE",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Casino",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 10000m,
                RegistrationDate = DateTime.Now,
                IsVerified = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            // Test speler
            var playerUser = new ApplicationUser
            {
                Id = "player-user-id",
                UserName = "speler@safecasino.be",
                NormalizedUserName = "SPELER@SAFECASINO.BE",
                Email = "speler@safecasino.be",
                NormalizedEmail = "SPELER@SAFECASINO.BE",
                EmailConfirmed = true,
                FirstName = "Jan",
                LastName = "Janssens",
                DateOfBirth = new DateTime(1995, 5, 15),
                Balance = 500m,
                RegistrationDate = DateTime.Now,
                IsVerified = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            playerUser.PasswordHash = hasher.HashPassword(playerUser, "Speler123!");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser, playerUser);

            // Gebruiker-rol koppelingen
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", // Admin rol
                    UserId = adminUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = "3", // Player rol
                    UserId = playerUser.Id
                }
            );
        }
    }
}