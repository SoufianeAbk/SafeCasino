using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace SafeCasino.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        public IActionResult SetCulture(string culture, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = "nl";
            }

            // Validate culture
            var supportedCultures = new[] { "nl", "en", "fr" };
            if (!supportedCultures.Contains(culture))
            {
                culture = "nl";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture, culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}