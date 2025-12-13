using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SafeCasino.Services;

namespace SafeCasino.Extensions
{
    /// <summary>
    /// Static helper for localization in Razor views
    /// </summary>
    public static class LocalizationHelper
    {
        private static ILocalizationService? _localizationService;
        private static HttpContext? _httpContext;

        /// <summary>
        /// Initialize the helper with dependencies (called automatically in _ViewImports)
        /// </summary>
        public static void Initialize(ILocalizationService localizationService, HttpContext httpContext)
        {
            _localizationService = localizationService;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Get the current language from cookie
        /// </summary>
        public static string GetCurrentLanguage()
        {
            if (_httpContext?.Request.Cookies["language"] != null)
            {
                return _httpContext.Request.Cookies["language"]!;
            }
            return "nl";
        }

        /// <summary>
        /// Get a localized string by key
        /// </summary>
        public static string GetString(string key)
        {
            if (_localizationService == null)
                return key;

            var language = GetCurrentLanguage();
            return _localizationService.GetString(key, language) ?? key;
        }

        /// <summary>
        /// Get a localized string by key with default value
        /// </summary>
        public static string GetString(string key, string defaultValue)
        {
            if (_localizationService == null)
                return defaultValue;

            var language = GetCurrentLanguage();
            var result = _localizationService.GetString(key, language);
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        // HTML Helper Extensions (for @Html.Localize syntax)

        public static string GetCurrentLanguage(this IHtmlHelper htmlHelper)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            return context.Request.Cookies["language"] ?? "nl";
        }

        public static IHtmlContent Localize(this IHtmlHelper htmlHelper, string key)
        {
            var localizationService = htmlHelper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ILocalizationService)) as ILocalizationService;

            if (localizationService == null)
                return new HtmlString(key);

            var language = GetCurrentLanguage(htmlHelper);
            var translatedString = localizationService.GetString(key, language);

            return new HtmlString(translatedString);
        }

        public static IHtmlContent Localize(this IHtmlHelper htmlHelper, string key, string defaultValue)
        {
            var localizationService = htmlHelper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ILocalizationService)) as ILocalizationService;

            if (localizationService == null)
                return new HtmlString(defaultValue);

            var language = GetCurrentLanguage(htmlHelper);
            var translatedString = localizationService.GetString(key, language);

            return new HtmlString(string.IsNullOrEmpty(translatedString) ? defaultValue : translatedString);
        }
    }
}