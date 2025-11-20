namespace SafeCasino.Services
{
    public interface IAuthenticationService
    {
        (bool Success, string UserType) ValidateCredentials(string username, string password);
        List<(string Code, string Username)> GetSupportedUsers();
    }
}
