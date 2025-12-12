using Microsoft.AspNetCore.Mvc;

namespace SafeCasino.Controllers
{
    public class LanguageController : Controller
    {
        /// <summary>
        /// Sets the language cookie and redirects back to the referring page
        /// </summary>
        /// <param name="lang">Language code (nl, en, fr)</param>
        [HttpGet("/Language/Set/{lang}")]
        public IActionResult Set(string lang)
        {
            // Validate language parameter
            var supportedLanguages = new[] { "nl", "en", "fr" };
            if (string.IsNullOrEmpty(lang) || !supportedLanguages.Contains(lang.ToLower()))
            {
                lang = "nl"; // Default to Dutch
            }

            // Set language cookie
            Response.Cookies.Append("language", lang.ToLower(), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),  // Cookie expires in 1 year
                HttpOnly = false,                              // Allow JavaScript access if needed
                Secure = false,                                // Set to true in production with HTTPS
                SameSite = SameSiteMode.Lax,                  // Protect against CSRF
                Path = "/"                                     // Available throughout the site
            });

            // Get the referring URL from headers
            string returnUrl = Request.Headers["Referer"].ToString();

            // If no referer, redirect to home page
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            // Redirect back to the page the user came from
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Gets the current language from cookie (optional endpoint for AJAX)
        /// </summary>
        [HttpGet("/Language/Current")]
        public IActionResult Current()
        {
            var currentLanguage = Request.Cookies["language"] ?? "nl";
            return Json(new { language = currentLanguage });
        }
    }
}