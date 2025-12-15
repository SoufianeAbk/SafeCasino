using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuratie voor GameCategory
    /// </summary>
    public class GameCategoryConfiguration : IEntityTypeConfiguration<GameCategory>
    {
        public void Configure(EntityTypeBuilder<GameCategory> builder)
        {
            // Tabel naam
            builder.ToTable("GameCategories");

            // Primary key
            builder.HasKey(gc => gc.Id);

            // Properties configuratie
            builder.Property(gc => gc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(gc => gc.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(gc => gc.IconUrl)
                .HasMaxLength(500);

            builder.Property(gc => gc.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(gc => gc.DisplayOrder)
                .IsRequired();

            // Index voor snellere queries
            builder.HasIndex(gc => gc.Name)
                .IsUnique();

            builder.HasIndex(gc => gc.DisplayOrder);
        }
    }
}