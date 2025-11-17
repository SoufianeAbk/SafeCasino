using Microsoft.AspNetCore.Mvc;

namespace SafeCasino.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult Change(string language, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = "nl";
            }

            // Set cookie voor 1 jaar
            Response.Cookies.Append("language", language, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            });

            // Redirect terug naar waar de gebruiker vandaan kwam
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult GetCurrent()
        {
            var language = Request.Cookies["language"] ?? "nl";
            return Json(new { language });
        }
    }
}