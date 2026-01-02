namespace SafeCasino.Api.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string email, string userName, string verificationLink);
        Task SendPasswordResetEmailAsync(string email, string userName, string resetLink);
        Task SendWelcomeEmailAsync(string email, string userName);
    }
}