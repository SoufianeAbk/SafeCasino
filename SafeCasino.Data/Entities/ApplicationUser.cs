using Microsoft.AspNetCore.Identity;

namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Custom gebruiker klasse die erft van IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Voornaam van de gebruiker
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Achternaam van de gebruiker
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Geboortedatum van de gebruiker (voor leeftijdsverificatie)
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Saldo van de gebruiker in het casino
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Registratiedatum van de gebruiker
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Of de gebruiker geverifieerd is
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Navigatie property voor reviews geschreven door deze gebruiker
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}