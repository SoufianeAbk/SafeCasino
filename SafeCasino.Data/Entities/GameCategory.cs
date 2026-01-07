using System;
using System.Collections.Generic;

namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Game Category - Categorie van casino spellen
    /// </summary>
    public class GameCategory
    {
        /// <summary>
        /// Unieke ID van de categorie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van de categorie
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschrijving van de categorie
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL naar icon van de categorie
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Is deze categorie actief?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Display volgorde
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Categorie aangemaakt op
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ============ NAVIGATION PROPERTIES ============

        /// <summary>
        /// Alle games in deze categorie
        /// One-to-Many relationship
        /// </summary>
        public virtual ICollection<CasinoGame> Games { get; set; } = new List<CasinoGame>();
    }
}