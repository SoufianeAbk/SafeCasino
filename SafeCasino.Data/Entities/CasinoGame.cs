using SafeCasino.Data.Identity;

namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Casino Game - Speelautomaat/spel in het casino
    /// </summary>
    public class CasinoGame
    {
        /// <summary>
        /// Unieke ID van het spel
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van het spel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschrijving van het spel
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL naar thumbnail afbeelding
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Minimale inzet voor dit spel
        /// </summary>
        public decimal MinimumBet { get; set; }

        /// <summary>
        /// Maximale inzet voor dit spel
        /// </summary>
        public decimal MaximumBet { get; set; }

        /// <summary>
        /// RTP (Return to Player) percentage
        /// </summary>
        public decimal RtpPercentage { get; set; }

        /// <summary>
        /// Is het spel beschikbaar?
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Aantal keren dat dit spel is gespeeld
        /// </summary>
        public int PlayCount { get; set; } = 0;

        /// <summary>
        /// Is dit een nieuw spel?
        /// </summary>
        public bool IsNew { get; set; } = false;

        /// <summary>
        /// Is dit een populair spel?
        /// </summary>
        public bool IsPopular { get; set; } = false;

        /// <summary>
        /// Spel aangemaakt op
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ============ FOREIGN KEYS ============

        /// <summary>
        /// ID van de provider (bv NetEnt, Playtech)
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// ID van de categorie (bv Slots, Roulette, Blackjack)
        /// </summary>
        public int CategoryId { get; set; }

        // ============ NAVIGATION PROPERTIES ============

        /// <summary>
        /// De provider van dit spel
        /// One-to-Many relationship (inverse)
        /// </summary>
        public virtual GameProvider Provider { get; set; }

        /// <summary>
        /// De categorie van dit spel
        /// One-to-Many relationship (inverse)
        /// </summary>
        public virtual GameCategory Category { get; set; }

        /// <summary>
        /// Gebruikers die dit spel als favoriet hebben gemarkeerd
        /// Many-to-Many relationship met ApplicationUser
        /// </summary>
        public virtual ICollection<ApplicationUser> FavoritedByUsers { get; set; } = new List<ApplicationUser>();

        /// <summary>
        /// Reviews op dit spel
        /// One-to-Many relationship
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}