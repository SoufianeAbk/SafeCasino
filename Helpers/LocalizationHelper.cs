using Microsoft.AspNetCore.Http;
using SafeCasino.Services;

namespace SafeCasino.Helpers
{
    public static class LocalizationHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static ILocalizationService? _localizationService;

        public static void Initialize(IHttpContextAccessor httpContextAccessor, ILocalizationService localizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
        }

        public static string Localize(string key)
        {
            if (_httpContextAccessor?.HttpContext == null || _localizationService == null)
                return key;

            var language = _httpContextAccessor.HttpContext.Request.Cookies["language"] ?? "nl";
            return _localizationService.GetString(key, language) ?? key;
        }
    }
}
