using Microsoft.AspNetCore.Identity;
using SafeCasino.Data.Entities;

namespace SafeCasino.Data.Identity
{
    /// <summary>
    /// Application User - uitbereiding van IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Voornaam van de gebruiker
        /// </summary>
        public string FirstName { get; set; } = string.Empty;  // ← Voeg dit toe

        /// <summary>
        /// Achternaam van de gebruiker
        /// </summary>
        public string LastName { get; set; } = string.Empty;  // ← Voeg dit toe

        /// <summary>
        /// Geboortedatum van de gebruiker
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Saldo van de gebruiker
        /// </summary>
        public decimal Balance { get; set; } = 0m;

        /// <summary>
        /// Is de gebruiker geverifieerd?
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Registratiedatum
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Account aangemaakt op
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Laatst ingelogd op
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Account actief?
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ============ NAVIGATION PROPERTIES ============

        /// <summary>
        /// Favoriete spellen van deze gebruiker
        /// Many-to-Many relationship met CasinoGame
        /// </summary>
        public virtual ICollection<CasinoGame> FavoriteGames { get; set; } = new List<CasinoGame>();

        /// <summary>
        /// Reviews geschreven door deze gebruiker
        /// One-to-Many relationship met Review
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}