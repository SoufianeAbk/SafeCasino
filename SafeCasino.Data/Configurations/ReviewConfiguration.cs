using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuratie voor Review
    /// </summary>
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Tabel naam
            builder.ToTable("Reviews");

            // Primary key
            builder.HasKey(r => r.Id);

            // Properties configuratie
            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.CreatedDate)
                .IsRequired();

            builder.Property(r => r.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            // Relaties
            builder.HasOne(r => r.Game)
                .WithMany(cg => cg.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(r => r.GameId);
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.Rating);
            builder.HasIndex(r => r.IsApproved);
        }
    }
}