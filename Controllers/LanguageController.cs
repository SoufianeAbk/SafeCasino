using Microsoft.AspNetCore.Mvc;

namespace SafeCasino.Controllers
{
    /// <summary>
    /// Controller for handling language switching
    /// Stores language preference in cookies
    /// </summary>
    public class LanguageController : Controller
    {
        /// <summary>
        /// Set the language and redirect back to previous page
        /// Usage: /Language/Set/en or /Language/Set/nl
        /// </summary>
        [HttpGet]
        public IActionResult Set(string language)
        {
            // Validate language code
            if (string.IsNullOrEmpty(language) || (language != "nl" && language != "en"))
            {
                language = "nl"; // Default to Dutch
            }

            // Set cookie to persist language choice (expires in 1 year)
            Response.Cookies.Append(
                "language",
                language,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    HttpOnly = false,
                    IsEssential = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                }
            );

            // Redirect back to referring page, or home if none
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Get current language (for AJAX requests)
        /// Returns JSON with current language code
        /// </summary>
        [HttpGet]
        public IActionResult GetCurrentLanguage()
        {
            var language = Request.Cookies["language"] ?? "nl";
            return Json(new { language });
        }
    }
}