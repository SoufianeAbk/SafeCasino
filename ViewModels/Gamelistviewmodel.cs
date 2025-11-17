using SafeCasino.Models;

namespace SafeCasino.ViewModels
{
    public class GameListViewModel
    {
        public List<Game> Games { get; set; } = new List<Game>();
        public FilterOptions Filters { get; set; } = new FilterOptions();
        public List<string> AvailableProviders { get; set; } = new List<string>();
        public List<GameCategory> AvailableCategories { get; set; } = new List<GameCategory>();
        public int TotalGames { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string CurrentLanguage { get; set; } = "nl";

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}