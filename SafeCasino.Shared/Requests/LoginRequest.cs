using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email adres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}