using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Web.ViewModels
{
    /// <summary>
    /// ViewModel voor gebruiker login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email adres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Onthoud mij")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}