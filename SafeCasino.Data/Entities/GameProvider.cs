namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// Entiteit voor game providers (bijv. NetEnt, Microgaming, etc.)
    /// </summary>
    public class GameProvider
    {
        /// <summary>
        /// Unieke identifier voor de provider
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Naam van de provider
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beschrijving van de provider
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Website URL van de provider
        /// </summary>
        public string WebsiteUrl { get; set; } = string.Empty;

        /// <summary>
        /// Logo URL van de provider
        /// </summary>
        public string LogoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Of de provider actief is
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigatie property voor games van deze provider
        /// </summary>
        public virtual ICollection<CasinoGame> Games { get; set; } = new List<CasinoGame>();
    }
}