namespace SafeCasino.Services
{
    /// <summary>
    /// Interface for localization service supporting multiple languages
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Get a translated string for the given key and language
        /// </summary>
        string GetString(string key, string language = "nl");

        /// <summary>
        /// Get all translations for a specific language
        /// </summary>
        Dictionary<string, string> GetAllStrings(string language = "nl");

        /// <summary>
        /// Get list of supported languages
        /// </summary>
        List<(string Code, string Name)> GetSupportedLanguages();
    }
}