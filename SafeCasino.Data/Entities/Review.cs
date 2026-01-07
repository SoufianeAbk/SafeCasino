using SafeCasino.Data.Identity;
using System;

namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Review - Review op een casino spel door een gebruiker
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unieke ID van de review
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Titel van de review
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Inhoud van de review
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Rating (1-5 sterren)
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Is deze review goedgekeurd door moderator?
        /// </summary>
        public bool IsApproved { get; set; } = false;

        /// <summary>
        /// Datum waarop review is aangemaakt
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Datum waarop review is gewijzigd (nullable)
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        // ============ FOREIGN KEYS ============

        /// <summary>
        /// ID van de gebruiker die deze review schreef
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ID van het spel waar deze review over gaat
        /// </summary>
        public int GameId { get; set; }

        // ============ NAVIGATION PROPERTIES ============

        /// <summary>
        /// De gebruiker die deze review schreef
        /// Many-to-One relationship
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Het spel waar deze review over gaat
        /// Many-to-One relationship
        /// </summary>
        public virtual CasinoGame Game { get; set; }
    }
}