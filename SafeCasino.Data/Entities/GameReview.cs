namespace SafeCasino.Data.Entities
{
    /// <summary>
    /// GameReview - alias voor Review entity
    /// Deze class maakt het mogelijk om DbSet<GameReview> te gebruiken
    /// terwijl het naar dezelfde tabel wijst als Review
    /// </summary>
    public class GameReview : Review
    {
        // Geen extra properties nodig
        // Deze class erft alles van Review en mapt naar dezelfde tabel
    }
}