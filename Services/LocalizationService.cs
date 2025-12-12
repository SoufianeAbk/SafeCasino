namespace SafeCasino.Services
{
    /// <summary>
    /// Service for managing translations in the application
    /// Supports Dutch (nl) and English (en)
    /// </summary>
    public class LocalizationService : ILocalizationService
    {
        // Dictionary structure: language -> (key -> translation)
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            ["nl"] = new Dictionary<string, string>
            {
                // Navigation & General
                ["Home"] = "Home",
                ["Games"] = "Spellen",
                ["Categories"] = "Categorieën",
                ["Popular"] = "Populair",
                ["New"] = "Nieuw",
                ["Jackpot"] = "Jackpot",
                ["FAQ"] = "Veelgestelde Vragen",
                ["Support"] = "Ondersteuning",
                ["Contact"] = "Contact",

                // User Account
                ["Login"] = "Inloggen",
                ["Logout"] = "Uitloggen",
                ["Register"] = "Registreren",
                ["Profile"] = "Mijn Profiel",
                ["Account"] = "Account",
                ["Settings"] = "Instellingen",
                ["ChangePassword"] = "Wachtwoord wijzigen",

                // Admin
                ["AdminLogin"] = "Admin Login",
                ["AdminPanel"] = "Admin Paneel",
                ["Dashboard"] = "Dashboard",
                ["Users"] = "Gebruikers",
                ["Manage"] = "Beheer",

                // Game Related
                ["MinBet"] = "Min. Inzet",
                ["MaxBet"] = "Max. Inzet",
                ["RTP"] = "RTP",
                ["PlayDemo"] = "Demo Spelen",
                ["PlayReal"] = "Echt Geld",
                ["Play"] = "Spelen",
                ["Description"] = "Beschrijving",
                ["Filter"] = "Filteren",
                ["ClearFilters"] = "Filters Wissen",
                ["SortBy"] = "Sorteren op",
                ["Provider"] = "Provider",
                ["Theme"] = "Thema",
                ["Genre"] = "Genre",

                // Messages
                ["Welcome"] = "Welkom bij SafeCasino",
                ["WelcomeMessage"] = "Ontdek de beste casinospellen in een veilige omgeving",
                ["StartPlaying"] = "Begin met Spelen",
                ["LatestGames"] = "Nieuwste Spellen",
                ["TopProviders"] = "Toppers",
                ["SafeGaming"] = "Veilig Spelen",
                ["Support247"] = "24/7 Ondersteuning",
                ["FastPayouts"] = "Snelle Uitbetalingen",
                ["MobileReady"] = "Mobiel Optimaal",
                ["NoGamesFound"] = "Geen spellen gevonden",
                ["ShowMore"] = "Meer Weergeven",
                ["ShowLess"] = "Minder Weergeven",

                // Pagination & Search
                ["Previous"] = "Vorige",
                ["Next"] = "Volgende",
                ["Page"] = "Pagina",
                ["Of"] = "van",
                ["ShowingResults"] = "Resultaten",
                ["TotalGames"] = "Totale spellen",
                ["Search"] = "Zoeken",

                // Form Fields
                ["Username"] = "Gebruikersnaam",
                ["Email"] = "E-mailadres",
                ["Password"] = "Wachtwoord",
                ["ConfirmPassword"] = "Wachtwoord bevestigen",
                ["FirstName"] = "Voornaam",
                ["LastName"] = "Achternaam",
                ["Language"] = "Taal",

                // Buttons
                ["Submit"] = "Verzenden",
                ["Cancel"] = "Annuleren",
                ["Save"] = "Opslaan",
                ["Delete"] = "Verwijderen",
                ["Edit"] = "Bewerken",
                ["Back"] = "Terug",
                ["Close"] = "Sluiten",

                // Error Messages
                ["Error"] = "Fout",
                ["Warning"] = "Waarschuwing",
                ["Success"] = "Succes",
                ["InvalidCredentials"] = "Ongeldige inloggegevens",
                ["RequiredField"] = "Dit veld is verplicht",
                ["InvalidEmail"] = "Ongeldig e-mailadres",
                ["PasswordMismatch"] = "Wachtwoorden komen niet overeen",
                ["UserNotFound"] = "Gebruiker niet gevonden",
                ["ErrorLoadingGames"] = "Fout bij laden van spellen",

                // Footer
                ["Copyright"] = "© SafeCasino. Alle rechten voorbehouden.",
                ["PrivacyPolicy"] = "Privacybeleid",
                ["TermsOfService"] = "Servicevoorwaarden",
                ["Responsible Gambling"] = "Verantwoord Gokken",
            },
            ["en"] = new Dictionary<string, string>
            {
                // Navigation & General
                ["Home"] = "Home",
                ["Games"] = "Games",
                ["Categories"] = "Categories",
                ["Popular"] = "Popular",
                ["New"] = "New",
                ["Jackpot"] = "Jackpot",
                ["FAQ"] = "FAQ",
                ["Support"] = "Support",
                ["Contact"] = "Contact",

                // User Account
                ["Login"] = "Login",
                ["Logout"] = "Logout",
                ["Register"] = "Register",
                ["Profile"] = "My Profile",
                ["Account"] = "Account",
                ["Settings"] = "Settings",
                ["ChangePassword"] = "Change Password",

                // Admin
                ["AdminLogin"] = "Admin Login",
                ["AdminPanel"] = "Admin Panel",
                ["Dashboard"] = "Dashboard",
                ["Users"] = "Users",
                ["Manage"] = "Manage",

                // Game Related
                ["MinBet"] = "Min. Bet",
                ["MaxBet"] = "Max. Bet",
                ["RTP"] = "RTP",
                ["PlayDemo"] = "Play Demo",
                ["PlayReal"] = "Real Money",
                ["Play"] = "Play",
                ["Description"] = "Description",
                ["Filter"] = "Filter",
                ["ClearFilters"] = "Clear Filters",
                ["SortBy"] = "Sort By",
                ["Provider"] = "Provider",
                ["Theme"] = "Theme",
                ["Genre"] = "Genre",

                // Messages
                ["Welcome"] = "Welcome to SafeCasino",
                ["WelcomeMessage"] = "Discover the best casino games in a safe environment",
                ["StartPlaying"] = "Start Playing",
                ["LatestGames"] = "Latest Games",
                ["TopProviders"] = "Top Providers",
                ["SafeGaming"] = "Safe Gaming",
                ["Support247"] = "24/7 Support",
                ["FastPayouts"] = "Fast Payouts",
                ["MobileReady"] = "Mobile Ready",
                ["NoGamesFound"] = "No games found",
                ["ShowMore"] = "Show More",
                ["ShowLess"] = "Show Less",

                // Pagination & Search
                ["Previous"] = "Previous",
                ["Next"] = "Next",
                ["Page"] = "Page",
                ["Of"] = "of",
                ["ShowingResults"] = "Results",
                ["TotalGames"] = "Total games",
                ["Search"] = "Search",

                // Form Fields
                ["Username"] = "Username",
                ["Email"] = "Email Address",
                ["Password"] = "Password",
                ["ConfirmPassword"] = "Confirm Password",
                ["FirstName"] = "First Name",
                ["LastName"] = "Last Name",
                ["Language"] = "Language",

                // Buttons
                ["Submit"] = "Submit",
                ["Cancel"] = "Cancel",
                ["Save"] = "Save",
                ["Delete"] = "Delete",
                ["Edit"] = "Edit",
                ["Back"] = "Back",
                ["Close"] = "Close",

                // Error Messages
                ["Error"] = "Error",
                ["Warning"] = "Warning",
                ["Success"] = "Success",
                ["InvalidCredentials"] = "Invalid credentials",
                ["RequiredField"] = "This field is required",
                ["InvalidEmail"] = "Invalid email address",
                ["PasswordMismatch"] = "Passwords do not match",
                ["UserNotFound"] = "User not found",
                ["ErrorLoadingGames"] = "Error loading games",

                // Footer
                ["Copyright"] = "© SafeCasino. All rights reserved.",
                ["PrivacyPolicy"] = "Privacy Policy",
                ["TermsOfService"] = "Terms of Service",
                ["Responsible Gambling"] = "Responsible Gambling",
            }
        };

        /// <summary>
        /// Get translated string for key in specified language
        /// Falls back to key if translation not found
        /// </summary>
        public string GetString(string key, string language = "nl")
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            // Default to "nl" if language not supported
            if (!_translations.ContainsKey(language))
                language = "nl";

            var langDict = _translations[language];
            return langDict.ContainsKey(key) ? langDict[key] : key;
        }

        /// <summary>
        /// Get all translations for a specific language
        /// </summary>
        public Dictionary<string, string> GetAllStrings(string language = "nl")
        {
            if (!_translations.ContainsKey(language))
                language = "nl";

            return new Dictionary<string, string>(_translations[language]);
        }

        /// <summary>
        /// Get list of supported languages with display names
        /// </summary>
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