using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuratie voor ApplicationUser
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Properties configuratie
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.Property(u => u.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(u => u.RegistrationDate)
                .IsRequired();

            builder.Property(u => u.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(u => u.Email);
            builder.HasIndex(u => u.UserName);
        }
    }
}