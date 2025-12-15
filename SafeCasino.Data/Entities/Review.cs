namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Entiteit voor gebruikersreviews van spellen
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unieke identifier voor de review
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Titel van de review
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Inhoud van de review
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Beoordeling (1-5 sterren)
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Datum waarop de review is geplaatst
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Of de review is goedgekeurd door moderatie
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Foreign key naar CasinoGame
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Navigatie property naar het spel
        /// </summary>
        public virtual CasinoGame Game { get; set; } = null!;

        /// <summary>
        /// Foreign key naar ApplicationUser
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigatie property naar de gebruiker
        /// </summary>
        public virtual ApplicationUser User { get; set; } = null!;
    }
}