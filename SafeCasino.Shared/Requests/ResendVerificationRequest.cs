using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Shared.Requests
{
    public class ResendVerificationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}