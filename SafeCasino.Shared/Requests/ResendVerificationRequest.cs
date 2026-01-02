using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests;

public class ResendVerificationRequest
{
    [Required(ErrorMessage = "Email is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig email adres")]
    public string Email { get; set; } = string.Empty;
}