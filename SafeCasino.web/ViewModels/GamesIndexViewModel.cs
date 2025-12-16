using SafeCasino.Data.Entities;

namespace SafeCasino.Web.ViewModels
{
    /// <summary>
    /// ViewModel voor games overzicht pagina
    /// </summary>
    public class GamesIndexViewModel
    {
        public List<CasinoGame> Games { get; set; } = new();
        public List<GameCategory> Categories { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }

        // Statistieken
        public int TotalGames { get; set; }
        public int PopularGamesCount { get; set; }
        public int NewGamesCount { get; set; }
    }
}