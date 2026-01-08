namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Gebruikers (Users)
    /// Gebruikt voor het uitwisseling van gebruiker gegevens tussen frontend en backend
    /// Bevat alleen publieke/relevante informatie, gevoelige gegevens blijven server-side
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unieke identifier van de gebruiker
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gebruikersnaam/email adres van de gebruiker
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email adres van de gebruiker
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Voornaam van de gebruiker
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Achternaam van de gebruiker
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Geboortedatum van de gebruiker
        /// Gebruikt voor leeftijdsverificatie
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Huidigsaldo van de gebruiker in het casino
        /// Dit is het bedrag dat beschikbaar is voor inzetten
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Datum waarop de gebruiker zich heeft ingeschreven
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Of de gebruiker zijn email adres heeft geverifieerd
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Of de gebruiker is geverifieerd als volwassene
        /// Vereist voor het spelen in het casino
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Rollen die deze gebruiker heeft
        /// Bijv. "Admin", "Moderator", "Player"
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Of het account momenteel vergrendeld is
        /// Dit kan gebeuren na te veel mislukte login pogingen
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Totaal aantal reviews dat deze gebruiker heeft geschreven
        /// </summary>
        public int ReviewCount { get; set; }
    }
}