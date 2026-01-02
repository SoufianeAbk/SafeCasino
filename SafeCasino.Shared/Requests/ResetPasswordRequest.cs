using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests;

public class ResetPasswordRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nieuw wachtwoord is verplicht")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Wachtwoord moet minimaal 8 karakters bevatten")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
}