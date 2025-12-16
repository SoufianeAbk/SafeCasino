namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Game Categories
    /// Gebruikt voor het uitwisselen van categorie gegevens tussen frontend en backend
    /// Categorieën zijn groepen van gerelateerde spellen (bijv. Blackjack, Roulette, Slots)
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Unieke identifier van de categorie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van de categorie (bijv. "Blackjack", "Slots", "Live Casino")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gedetailleerde beschrijving van de categorie
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL naar het icoon van de categorie
        /// Dit icoon wordt gebruikt in de UI voor visuele weergave
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Of deze categorie momenteel actief is en zichtbaar voor spelers
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Volgorde waarin de categorie wordt weergegeven in de UI
        /// Lagere nummers verschijnen eerst
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Aantal spellen in deze categorie
        /// </summary>
        public int GameCount { get; set; }
    }
}