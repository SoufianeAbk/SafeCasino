using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuratie voor CasinoGame
    /// </summary>
    public class CasinoGameConfiguration : IEntityTypeConfiguration<CasinoGame>
    {
        public void Configure(EntityTypeBuilder<CasinoGame> builder)
        {
            // Tabel naam
            builder.ToTable("CasinoGames");

            // Primary key
            builder.HasKey(cg => cg.Id);

            // Properties configuratie
            builder.Property(cg => cg.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(cg => cg.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(cg => cg.ThumbnailUrl)
                .HasMaxLength(500);

            builder.Property(cg => cg.MinimumBet)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cg => cg.MaximumBet)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cg => cg.RtpPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(cg => cg.IsAvailable)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(cg => cg.IsNew)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(cg => cg.IsPopular)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(cg => cg.CreatedDate)
                .IsRequired();

            builder.Property(cg => cg.PlayCount)
                .IsRequired()
                .HasDefaultValue(0);

            // Relaties
            builder.HasOne(cg => cg.Category)
                .WithMany(gc => gc.Games)
                .HasForeignKey(cg => cg.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cg => cg.Provider)
                .WithMany(gp => gp.Games)
                .HasForeignKey(cg => cg.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes voor betere performance
            builder.HasIndex(cg => cg.Name);
            builder.HasIndex(cg => cg.CategoryId);
            builder.HasIndex(cg => cg.ProviderId);
            builder.HasIndex(cg => cg.IsPopular);
            builder.HasIndex(cg => cg.IsNew);
        }
    }
}