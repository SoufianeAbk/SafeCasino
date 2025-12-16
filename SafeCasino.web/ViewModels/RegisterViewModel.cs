using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Web.ViewModels
{
    /// <summary>
    /// ViewModel voor nieuwe gebruiker registratie
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(100, ErrorMessage = "Voornaam mag maximaal 100 karakters zijn")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters zijn")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email adres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Wachtwoord moet minimaal {2} en maximaal {1} karakters zijn.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Je moet akkoord gaan met de voorwaarden")]
        [Display(Name = "Ik ga akkoord met de algemene voorwaarden")]
        public bool AcceptTerms { get; set; }
    }
}