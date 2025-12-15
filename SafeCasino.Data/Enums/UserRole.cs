namespace SafeCasino.Data.Enums
{
    /// <summary>
    /// Enum voor de verschillende gebruikersrollen in het casino systeem
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Standaard speler rol
        /// </summary>
        Player = 0,

        /// <summary>
        /// Administrator met volledige rechten
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Moderator met beperkte beheersrechten
        /// </summary>
        Moderator = 2
    }
}