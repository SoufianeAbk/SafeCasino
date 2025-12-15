namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Entiteit voor spelcategorieën (bijv. Blackjack, Slots, etc.)
    /// </summary>
    public class GameCategory
    {
        /// <summary>
        /// Unieke identifier voor de categorie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van de categorie
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beschrijving van de categorie
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL naar het icoon van de categorie
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Of de categorie actief is
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Sorteervolgorde voor weergave
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Navigatie property voor games in deze categorie
        /// </summary>
        public virtual ICollection<CasinoGame> Games { get; set; } = new List<CasinoGame>();
    }
}