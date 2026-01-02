using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}