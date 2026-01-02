using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Voornaam is verplicht")]
    [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 karakters bevatten")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Achternaam is verplicht")]
    [StringLength(50, ErrorMessage = "Achternaam mag maximaal 50 karakters bevatten")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Geboortedatum is verplicht")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Wachtwoord moet minimaal 8 karakters bevatten")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}