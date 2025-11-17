namespace SafeCasino.Services
{
    public interface ILocalizationService
    {
        string GetString(string key, string language = "nl");
        Dictionary<string, string> GetAllStrings(string language = "nl");
        List<(string Code, string Name)> GetSupportedLanguages();
    }
}