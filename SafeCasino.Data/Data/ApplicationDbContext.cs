using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Configurations;
using SafeCasino.Data.Entities;
using SafeCasino.Data.Identity;

namespace SafeCasino.Data.Data
{
    /// <summary>
    /// Database context voor de SafeCasino applicatie
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        /// <summary>
        /// Constructor met options
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet voor casino spellen
        /// </summary>
        public DbSet<CasinoGame> CasinoGames { get; set; }

        /// <summary>
        /// DbSet voor spelcategorieën
        /// </summary>
        public DbSet<GameCategory> GameCategories { get; set; }

        /// <summary>
        /// DbSet voor game providers
        /// </summary>
        public DbSet<GameProvider> GameProviders { get; set; }

        /// <summary>
        /// DbSet voor reviews
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// Configure warnings
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Suppress pending model changes warning (only for development)
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore
                    .Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        /// <summary>
        /// Model configuratie met Fluent API
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply alle configuraties
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new GameCategoryConfiguration());
            builder.ApplyConfiguration(new GameProviderConfiguration());
            builder.ApplyConfiguration(new CasinoGameConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());

            // Identity tabellen hernoemen voor duidelijkheid
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // Seed data toevoegen
            Seed.RoleSeed.SeedRoles(builder);
            Seed.UserSeed.SeedUsers(builder);
            Seed.DataSeed.SeedData(builder);
        }
    }
}