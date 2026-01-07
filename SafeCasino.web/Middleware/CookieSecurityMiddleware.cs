namespace SafeCasino.Web.Middleware
{
    /// <summary>
    /// Middleware voor beveiligde cookie handling
    /// </summary>
    public class CookieSecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CookieSecurityMiddleware> _logger;

        public CookieSecurityMiddleware(RequestDelegate next, ILogger<CookieSecurityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Voeg SameSite attribuut toe aan alle cookies
            var cookieOptions = context.Response.HttpContext.Request.Cookies
                .Select(c => c.Key)
                .ToList();

            _logger.LogDebug("🍪 Processing cookies: {CookieCount}", cookieOptions.Count);

            await _next(context);

            // Werk de Set-Cookie header bij met beveiligde instellingen
            var setCookieHeaders = context.Response.Headers.SetCookie.ToList();
            if (setCookieHeaders.Any())
            {
                context.Response.Headers.Remove("Set-Cookie");

                foreach (var header in setCookieHeaders)
                {
                    var cookieValue = header.ToString();

                    // Voeg SameSite toe als dit nog niet het geval is
                    if (!cookieValue.Contains("SameSite", StringComparison.OrdinalIgnoreCase))
                    {
                        cookieValue += "; SameSite=Strict";
                    }

                    // Voeg Secure flag toe voor HTTPS
                    if (context.Request.IsHttps && !cookieValue.Contains("Secure", StringComparison.OrdinalIgnoreCase))
                    {
                        cookieValue += "; Secure";
                    }

                    // Voeg HttpOnly flag toe (beschermt tegen XSS)
                    if (!cookieValue.Contains("HttpOnly", StringComparison.OrdinalIgnoreCase))
                    {
                        cookieValue += "; HttpOnly";
                    }

                    context.Response.Headers.Append("Set-Cookie", cookieValue);
                    _logger.LogDebug("🔐 Cookie beveiligd: {CookieName}", ExtractCookieName(cookieValue));
                }
            }
        }

        /// <summary>
        /// Extraheert de cookie naam uit de Set-Cookie header
        /// </summary>
        private static string ExtractCookieName(string cookieHeader)
        {
            var parts = cookieHeader.Split('=');
            return parts.Length > 0 ? parts[0].Trim() : "Unknown";
        }
    }

    /// <summary>
    /// Extension method om middleware toe te voegen
    /// </summary>
    public static class CookieSecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieSecurity(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieSecurityMiddleware>();
        }
    }
}