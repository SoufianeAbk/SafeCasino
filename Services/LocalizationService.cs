namespace SafeCasino.Services
{
    public interface ILocalizationService
    {
        string GetString(string key, string language = "nl");
        Dictionary<string, string> GetAllStrings(string language = "nl");
        List<(string Code, string Name)> GetSupportedLanguages();
    }

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
                ["Jackpot"] = "Jackpot",
                ["AllGames"] = "Alle Spellen",
                ["Search"] = "Zoeken",
                ["SearchGames"] = "Zoek spellen...",
                ["Category"] = "Categorie",
                ["Provider"] = "Provider",
                ["MinBet"] = "Min. Inzet",
                ["MaxBet"] = "Max. Inzet",
                ["Filter"] = "Filteren",
                ["ClearFilters"] = "Filters Wissen",
                ["Play"] = "Spelen",
                ["PlayDemo"] = "Demo Spelen",
                ["PlayReal"] = "Echt Geld",
                ["RTP"] = "RTP",
                ["Description"] = "Beschrijving",
                ["RelatedGames"] = "Vergelijkbare Spellen",
                ["BackToGames"] = "Terug naar Spellen",
                ["NoGamesFound"] = "Geen spellen gevonden",
                ["ShowMore"] = "Meer Tonen",
                ["ShowLess"] = "Minder Tonen",
                ["Language"] = "Taal",
                ["Welcome"] = "Welkom bij SafeCasino",
                ["WelcomeMessage"] = "Ontdek de beste casino spellen in een veilige omgeving",
                ["StartPlaying"] = "Begin met Spelen",
                ["LatestGames"] = "Nieuwste Spellen",
                ["TopProviders"] = "Top Providers",
                ["SafeGaming"] = "Veilig Spelen",
                ["Support247"] = "24/7 Support",
                ["FastPayouts"] = "Snelle Uitbetalingen",
                ["MobileReady"] = "Mobiel Vriendelijk",
                ["Previous"] = "Vorige",
                ["Next"] = "Volgende",
                ["Page"] = "Pagina",
                ["Of"] = "van",
                ["ShowingResults"] = "Resultaten",
                ["TotalGames"] = "Totaal aantal spellen"
            },
            ["en"] = new Dictionary<string, string>
            {
                ["Home"] = "Home",
                ["Games"] = "Games",
                ["Popular"] = "Popular",
                ["New"] = "New",
                ["Jackpot"] = "Jackpot",
                ["AllGames"] = "All Games",
                ["Search"] = "Search",
                ["SearchGames"] = "Search games...",
                ["Category"] = "Category",
                ["Provider"] = "Provider",
                ["MinBet"] = "Min. Bet",
                ["MaxBet"] = "Max. Bet",
                ["Filter"] = "Filter",
                ["ClearFilters"] = "Clear Filters",
                ["Play"] = "Play",
                ["PlayDemo"] = "Play Demo",
                ["PlayReal"] = "Real Money",
                ["RTP"] = "RTP",
                ["Description"] = "Description",
                ["RelatedGames"] = "Related Games",
                ["BackToGames"] = "Back to Games",
                ["NoGamesFound"] = "No games found",
                ["ShowMore"] = "Show More",
                ["ShowLess"] = "Show Less",
                ["Language"] = "Language",
                ["Welcome"] = "Welcome to SafeCasino",
                ["WelcomeMessage"] = "Discover the best casino games in a safe environment",
                ["StartPlaying"] = "Start Playing",
                ["LatestGames"] = "Latest Games",
                ["TopProviders"] = "Top Providers",
                ["SafeGaming"] = "Safe Gaming",
                ["Support247"] = "24/7 Support",
                ["FastPayouts"] = "Fast Payouts",
                ["MobileReady"] = "Mobile Ready",
                ["Previous"] = "Previous",
                ["Next"] = "Next",
                ["Page"] = "Page",
                ["Of"] = "of",
                ["ShowingResults"] = "Results",
                ["TotalGames"] = "Total games"
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

            // Fallback to key if translation not found
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