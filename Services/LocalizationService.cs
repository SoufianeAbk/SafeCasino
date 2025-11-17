namespace SafeCasino.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            ["nl"] = new Dictionary<string, string>
            {
                ["Home"] = "Home",
                ["Games"] = "Spellen",
                ["Popular"] = "Populair",
                ["New"] = "Nieuw",
                ["Jackpot"] = "Jackpot"
            },
            ["en"] = new Dictionary<string, string>
            {
                ["Home"] = "Home",
                ["Games"] = "Games",
                ["Popular"] = "Popular",
                ["New"] = "New",
                ["Jackpot"] = "Jackpot"
            }
        };

        public string GetString(string key, string language = "nl")
        {
            if (_translations.TryGetValue(language, out var langDict))
            {
                if (langDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            return key;
        }

        public Dictionary<string, string> GetAllStrings(string language = "nl")
        {
            return _translations.TryGetValue(language, out var langDict)
                ? new Dictionary<string, string>(langDict)
                : new Dictionary<string, string>();
        }

        public List<(string Code, string Name)> GetSupportedLanguages()
        {
            return new List<(string, string)>
            {
                ("nl", "Nederlands"),
                ("en", "English")
            };
        }
    }
}