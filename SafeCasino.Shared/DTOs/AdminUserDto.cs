namespace SafeCasino.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object voor Admin User Management
    /// Bevat uitgebreide gebruiker informatie voor administrators
    /// Gebruikt voor het beheren van gebruikers in het admin dashboard
    /// </summary>
    public class AdminUserDto
    {
        /// <summary>
        /// Unieke identifier van de gebruiker
        /// </summary>
        public string Id { get; set; } = string.Empty;

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
        /// Huidig saldo van de gebruiker in het casino
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
        /// Of de gebruiker is geverifieerd als volwassene (18+)
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Of het account actief is
        /// Inactieve accounts kunnen niet inloggen
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Of het account momenteel vergrendeld is (lockout)
        /// Dit kan gebeuren na te veel mislukte login pogingen of door admin actie
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Rollen die deze gebruiker heeft
        /// Bijvoorbeeld: "Admin", "Moderator", "Player"
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Totaal aantal reviews dat deze gebruiker heeft geschreven
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Volledige naam (voornaam + achternaam)
        /// Computed property voor gemakkelijke weergave
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Leeftijd van de gebruiker (berekend op basis van geboortedatum)
        /// </summary>
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;

                var age = DateTime.Today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > DateTime.Today.AddYears(-age))
                    age--;

                return age;
            }
        }

        /// <summary>
        /// Aantal dagen sinds registratie
        /// </summary>
        public int DaysSinceRegistration => (DateTime.Now - RegistrationDate).Days;
    }
}