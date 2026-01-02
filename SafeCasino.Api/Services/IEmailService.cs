using SafeCasino.Data.Entities;

namespace SafeCasino.Api.Services;

public interface IEmailService
{
    Task SendEmailVerificationAsync(ApplicationUser user, string token);
    Task SendPasswordResetEmailAsync(ApplicationUser user, string token);
    Task SendWelcomeEmailAsync(ApplicationUser user);
}