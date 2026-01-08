using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests
{
    /// <summary>
    /// Request model voor het bijwerken van gebruiker gegevens door een administrator
    /// Administrators kunnen meer velden wijzigen dan gewone gebruikers
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Voornaam van de gebruiker
        /// </summary>
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(100, ErrorMessage = "Voornaam mag maximaal 100 karakters zijn")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Achternaam van de gebruiker
        /// </summary>
        [Required(ErrorMessage = "Achternaam is verplicht")]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters zijn")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Saldo van de gebruiker
        /// Admin kan saldo direct wijzigen (normaal via payment provider)
        /// </summary>
        [Required(ErrorMessage = "Saldo is verplicht")]
        [Range(0, double.MaxValue, ErrorMessage = "Saldo moet positief zijn")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Of het account actief is
        /// Admin kan accounts deactiveren zonder ze te verwijderen
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Of de gebruiker is geverifieerd als volwassene
        /// Admin kan dit forceren voor test accounts of handmatige verificatie
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Of het email adres is bevestigd
        /// Admin kan dit forceren om verificatie mail te bypassen
        /// </summary>
        public bool EmailConfirmed { get; set; }
    }
}