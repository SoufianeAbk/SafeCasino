using SafeCasino.Data.Identity;

namespace SafeCasino.Api.Services;

public interface IEmailService
{
    Task SendEmailVerificationAsync(ApplicationUser user, string token);
    Task SendPasswordResetEmailAsync(ApplicationUser user, string token);
    Task SendWelcomeEmailAsync(ApplicationUser user);
}