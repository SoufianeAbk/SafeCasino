namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Reviews (beoordelingen van spelen)
    /// Gebruikt voor het uitwisselen van review gegevens tussen frontend en backend
    /// Spelers kunnen spellen beoordelen met een sterrenrating en beschrijving
    /// </summary>
    public class ReviewDto
    {
        /// <summary>
        /// Unieke identifier van de review
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Titel van de review
        /// Korte samenvatting van de mening van de speler
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Volledige inhoud/tekst van de review
        /// Gedetailleerde beschrijving van de ervaringen van de speler
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Beoordeling in sterren (1-5)
        /// 1 = zeer slecht, 5 = uitstekend
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Datum en tijd waarop de review is geplaatst
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Of de review is goedgekeurd door een moderator
        /// Alleen goedgekeurde reviews worden publiek zichtbaar
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// ID van het spel waarover de review gaat
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Naam van het spel waarover de review gaat
        /// </summary>
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// ID van de gebruiker die de review heeft geschreven
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Voornaam en achternaam van de reviewer
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gemiddelde rating van alle reviews voor dit spel
        /// Handig voor display op het spel detailpagina
        /// </summary>
        public decimal? AverageRating { get; set; }

        /// <summary>
        /// Totaal aantal reviews voor dit spel
        /// </summary>
        public int TotalReviews { get; set; }
    }
}