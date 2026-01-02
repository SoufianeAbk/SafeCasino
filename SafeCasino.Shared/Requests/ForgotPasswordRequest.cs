using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    public string Email { get; set; } = string.Empty;
}