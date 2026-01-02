using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email adres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 karakters zijn")]
        public string Password { get; set; } = string.Empty;
    }
}