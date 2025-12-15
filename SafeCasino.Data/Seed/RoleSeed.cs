using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Identity;

namespace SafeCasino.Data.Seed
{
    /// <summary>
    /// Seed data voor rollen
    /// </summary>
    public static class RoleSeed
    {
        /// <summary>
        /// Seed de rollen in de database
        /// </summary>
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator met volledige toegang tot het systeem",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new ApplicationRole
                {
                    Id = "2",
                    Name = "Moderator",
                    NormalizedName = "MODERATOR",
                    Description = "Moderator met beperkte beheersrechten",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new ApplicationRole
                {
                    Id = "3",
                    Name = "Player",
                    NormalizedName = "PLAYER",
                    Description = "Standaard speler rol",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }
}