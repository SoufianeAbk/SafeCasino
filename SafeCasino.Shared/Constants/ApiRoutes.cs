namespace SafeCasino.Shared.Constants
{
    /// <summary>
    /// Bevat alle API route constanten voor communicatie tussen frontend en backend
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// Base URL voor alle API calls
        /// </summary>
        public const string BaseUrl = "api";

        /// <summary>
        /// Routes voor casino games
        /// </summary>
        public static class Games
        {
            public const string Controller = $"{BaseUrl}/games";
            public const string GetAll = $"{Controller}";
            public const string GetById = $"{Controller}/{{id}}";
            public const string GetByCategory = $"{Controller}/category/{{categoryId}}";
            public const string GetPopular = $"{Controller}/popular";
            public const string GetNew = $"{Controller}/new";
            public const string Create = $"{Controller}";
            public const string Update = $"{Controller}/{{id}}";
            public const string Delete = $"{Controller}/{{id}}";
        }

        /// <summary>
        /// Routes voor spelcategorieën
        /// </summary>
        public static class Categories
        {
            public const string Controller = $"{BaseUrl}/categories";
            public const string GetAll = $"{Controller}";
            public const string GetById = $"{Controller}/{{id}}";
            public const string Create = $"{Controller}";
            public const string Update = $"{Controller}/{{id}}";
            public const string Delete = $"{Controller}/{{id}}";
        }

        /// <summary>
        /// Routes voor game providers
        /// </summary>
        public static class Providers
        {
            public const string Controller = $"{BaseUrl}/providers";
            public const string GetAll = $"{Controller}";
            public const string GetById = $"{Controller}/{{id}}";
            public const string Create = $"{Controller}";
            public const string Update = $"{Controller}/{{id}}";
            public const string Delete = $"{Controller}/{{id}}";
        }

        /// <summary>
        /// Routes voor reviews
        /// </summary>
        public static class Reviews
        {
            public const string Controller = $"{BaseUrl}/reviews";
            public const string GetByGame = $"{Controller}/game/{{gameId}}";
            public const string Create = $"{Controller}";
            public const string Update = $"{Controller}/{{id}}";
            public const string Delete = $"{Controller}/{{id}}";
            public const string Approve = $"{Controller}/{{id}}/approve";
        }

        /// <summary>
        /// Routes voor gebruikers
        /// </summary>
        public static class Users
        {
            public const string Controller = $"{BaseUrl}/users";
            public const string GetProfile = $"{Controller}/profile";
            public const string UpdateProfile = $"{Controller}/profile";
            public const string GetBalance = $"{Controller}/balance";
            public const string UpdateBalance = $"{Controller}/balance";
        }

        /// <summary>
        /// Routes voor authenticatie
        /// </summary>
        public static class Auth
        {
            public const string Controller = $"{BaseUrl}/auth";
            public const string Login = $"{Controller}/login";
            public const string Register = $"{Controller}/register";
            public const string Logout = $"{Controller}/logout";
            public const string ChangePassword = $"{Controller}/change-password";
        }
    }
}