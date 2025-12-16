using SafeCasino.Data.Entities;

namespace SafeCasino.Web.ViewModels
{
    /// <summary>
    /// ViewModel voor game details pagina
    /// </summary>
    public class GameDetailsViewModel
    {
        public CasinoGame Game { get; set; } = null!;
        public List<Review> Reviews { get; set; } = new();
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public bool CanReview { get; set; }
        public bool UserHasReviewed { get; set; }
    }
}