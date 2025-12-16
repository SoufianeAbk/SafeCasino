namespace SafeCasino.Shared.Requests
{
    /// <summary>
    /// Request model voor het aanmaken van een nieuw casino spel
    /// Dit wordt gebruikt wanneer een administrator een nieuw spel aan het systeem toevoegt
    /// </summary>
    public class CreateGameRequest
    {
        /// <summary>
        /// Naam van het spel
        /// Bijvoorbeeld: "Classic Blackjack", "Starburst Slot"
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beschrijving van het spel
        /// Gedetailleerde uitleg van het speltype, features en regels
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL naar de thumbnail/preview afbeelding van het spel
        /// Dit wordt gebruikt als preview in het gamemenu
        /// </summary>
        public string ThumbnailUrl { get; set; } = string.Empty;

        /// <summary>
        /// Minimale inzet amount in euro's
        /// Laagste bedrag dat een speler in kan zetten
        /// </summary>
        public decimal MinimumBet { get; set; }

        /// <summary>
        /// Maximale inzet amount in euro's
        /// Hoogste bedrag dat een speler in kan zetten
        /// </summary>
        public decimal MaximumBet { get; set; }

        /// <summary>
        /// Return to Player percentage (RTP)
        /// Percentage van inzetten dat statistisch gezien aan spelers wordt teruggegeven
        /// Bijvoorbeeld: 96.5 voor 96.5% RTP
        /// </summary>
        public decimal RtpPercentage { get; set; }

        /// <summary>
        /// Of het spel beschikbaar moet zijn voor spelers
        /// Spelen kunnen tijdelijk uitgeschakeld worden
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Of het spel als nieuw moet worden gemarkeerd
        /// Nieuwe spelen krijgen extra aandacht in de UI
        /// </summary>
        public bool IsNew { get; set; } = false;

        /// <summary>
        /// Of het spel als populair moet worden gemarkeerd
        /// Populaire spelen worden prominent weergegeven
        /// </summary>
        public bool IsPopular { get; set; } = false;

        /// <summary>
        /// ID van de spelcategorie waar dit spel onder valt
        /// Bijvoorbeeld: 1 voor Blackjack, 5 voor Slots
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// ID van de game provider die dit spel levert
        /// Bijvoorbeeld: 1 voor NetEnt, 3 voor Microgaming
        /// </summary>
        public int ProviderId { get; set; }
    }
}