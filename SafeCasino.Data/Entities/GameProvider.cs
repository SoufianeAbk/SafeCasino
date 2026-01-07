using System;
using System.Collections.Generic;

namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Game Provider - Aanbieder van casino spellen
    /// </summary>
    public class GameProvider
    {
        /// <summary>
        /// Unieke ID van de provider
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van de provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschrijving van de provider
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL naar logo van de provider
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Website URL van de provider
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Is deze provider actief?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Provider aangemaakt op
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ============ NAVIGATION PROPERTIES ============

        /// <summary>
        /// Alle games van deze provider
        /// One-to-Many relationship
        /// </summary>
        public virtual ICollection<CasinoGame> Games { get; set; } = new List<CasinoGame>();
    }
}