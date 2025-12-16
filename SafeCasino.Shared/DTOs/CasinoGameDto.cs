namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Casino Games
    /// Gebruikt voor het uitwisselen van spelgegevens tussen frontend en backend
    /// </summary>
    public class CasinoGameDto
    {
        /// <summary>
        /// Unieke identifier van het spel
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van het spel
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beschrijving van het spel
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL naar de thumbnail/preview afbeelding van het spel
        /// </summary>
        public string ThumbnailUrl { get; set; } = string.Empty;

        /// <summary>
        /// Minimale inzet voor dit spel
        /// </summary>
        public decimal MinimumBet { get; set; }

        /// <summary>
        /// Maximale inzet voor dit spel
        /// </summary>
        public decimal MaximumBet { get; set; }

        /// <summary>
        /// Return to Player percentage (RTP) - hoeveel % van inzetten wordt teruggegeven
        /// </summary>
        public decimal RtpPercentage { get; set; }

        /// <summary>
        /// Of het spel momenteel beschikbaar is voor spelers
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Of het spel als nieuw wordt gemarkeerd
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Of het spel populair is (vaak gespeeld)
        /// </summary>
        public bool IsPopular { get; set; }

        /// <summary>
        /// Datum waarop het spel is toegevoegd aan het casino
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Totaal aantal keren dat dit spel is gespeeld
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// ID van de spelcategorie (bijv. Blackjack, Slots, etc.)
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Naam van de spelcategorie
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// ID van de game provider
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Naam van de game provider
        /// </summary>
        public string ProviderName { get; set; } = string.Empty;
    }
}