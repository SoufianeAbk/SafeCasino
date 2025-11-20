namespace SafeCasino.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Dictionary<string, (string Password, string UserType)> _users = new()
        {
            { "casinouser@ehb.be", ("User!321", "User") },
            { "casinoadmin@ehb.be", ("Admin!321", "Admin") }
        };

        public (bool Success, string UserType) ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, string.Empty);
            }

            if (_users.TryGetValue(username, out var user))
            {
                if (user.Password == password)
                {
                    return (true, user.UserType);
                }
            }

            return (false, string.Empty);
        }

        public List<(string Code, string Username)> GetSupportedUsers()
        {
            return new List<(string, string)>
            {
                ("User", "casinouser@ehb.be"),
                ("Admin", "casinoadmin@ehb.be")
            };
        }
    }
}
