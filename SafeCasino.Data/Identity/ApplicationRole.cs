using Microsoft.AspNetCore.Identity;

namespace SafeCasino.Data.Identity
{
    /// <summary>
    /// Application Role - uitbereiding van IdentityRole
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Beschrijving van de rol
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Datum waarop rol is aangemaakt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Is deze rol actief?
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}