using Microsoft.EntityFrameworkCore;
using SafeCasino.Data;
using SafeCasino.Models;

namespace SafeCasino.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SafeCasinoDbContext _dbContext;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(SafeCasinoDbContext dbContext, ILogger<AuthenticationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public (bool Success, string UserType) ValidateCredentials(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return (false, string.Empty);
                }

                // Find user in database
                var user = _dbContext.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt for non-existent user: {Username}", username);
                    return (false, string.Empty);
                }

                // Simple password validation (in production, use proper hashing like bcrypt)
                if (user.PasswordHash == password)
                {
                    _logger.LogInformation("Successful login for user: {Username}", username);
                    return (true, user.UserType);
                }

                _logger.LogWarning("Failed login attempt for user: {Username}", username);
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating credentials for user: {Username}", username);
                return (false, string.Empty);
            }
        }

        public List<(string Code, string Username)> GetSupportedUsers()
        {
            try
            {
                return _dbContext.Users
                    .Select(u => (u.UserType, u.Username))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supported users");
                return new List<(string, string)>();
            }
        }
    }
}