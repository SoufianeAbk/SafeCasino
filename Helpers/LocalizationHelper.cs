using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SafeCasino.Services;

namespace SafeCasino.Extensions
{
    /// <summary>
    /// HTML Helper extension for localization
    /// Usage in views: @Html.Localize("Key")
    /// </summary>
    public static class LocalizationHelper
    {
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