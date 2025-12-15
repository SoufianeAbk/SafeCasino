using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuratie voor GameProvider
    /// </summary>
    public class GameProviderConfiguration : IEntityTypeConfiguration<GameProvider>
    {
        public void Configure(EntityTypeBuilder<GameProvider> builder)
        {
            // Tabel naam
            builder.ToTable("GameProviders");

            // Primary key
            builder.HasKey(gp => gp.Id);

            // Properties configuratie
            builder.Property(gp => gp.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(gp => gp.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(gp => gp.WebsiteUrl)
                .HasMaxLength(500);

            builder.Property(gp => gp.LogoUrl)
                .HasMaxLength(500);

            builder.Property(gp => gp.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Index voor unieke naam
            builder.HasIndex(gp => gp.Name)
                .IsUnique();
        }
    }
}