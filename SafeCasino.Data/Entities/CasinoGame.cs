namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Entiteit voor casino spellen
    /// </summary>
    public class CasinoGame
    {
        /// <summary>
        /// Unieke identifier voor het spel
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
        /// URL naar de thumbnail van het spel
        /// </summary>
        public string ThumbnailUrl { get; set; } = string.Empty;

        /// <summary>
        /// Minimum inzet voor het spel
        /// </summary>
        public decimal MinimumBet { get; set; }

        /// <summary>
        /// Maximum inzet voor het spel
        /// </summary>
        public decimal MaximumBet { get; set; }

        /// <summary>
        /// Return to Player percentage (RTP)
        /// </summary>
        public decimal RtpPercentage { get; set; }

        /// <summary>
        /// Of het spel momenteel beschikbaar is
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Of het spel nieuw is
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Of het spel populair is
        /// </summary>
        public bool IsPopular { get; set; }

        /// <summary>
        /// Datum waarop het spel is toegevoegd
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Aantal keer dat het spel is gespeeld
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// Foreign key naar GameCategory
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Navigatie property naar de categorie
        /// </summary>
        public virtual GameCategory Category { get; set; } = null!;

        /// <summary>
        /// Foreign key naar GameProvider
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Navigatie property naar de provider
        /// </summary>
        public virtual GameProvider Provider { get; set; } = null!;

        /// <summary>
        /// Navigatie property voor reviews van dit spel
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}