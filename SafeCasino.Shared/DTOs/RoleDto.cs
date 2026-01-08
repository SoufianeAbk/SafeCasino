namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Role Management
    /// Bevat informatie over rollen in het systeem
    /// Gebruikt voor het beheren van rollen in het admin dashboard
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Unieke identifier van de rol
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Naam van de rol
        /// Bijvoorbeeld: "Admin", "Moderator", "Player"
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beschrijving van de rol
        /// Legt uit wat de rechten en verantwoordelijkheden van deze rol zijn
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Of deze rol actief is
        /// Inactieve rollen kunnen niet toegewezen worden aan gebruikers
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Aantal gebruikers dat deze rol heeft
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// Datum waarop de rol is aangemaakt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Is dit een systeem rol die niet verwijderd mag worden?
        /// Bijvoorbeeld: Admin, Player zijn systeem rollen
        /// </summary>
        public bool IsSystemRole => Name is "Admin" or "Player" or "Moderator";
    }
}