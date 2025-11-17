using SafeCasino.Models;

namespace SafeCasino.ViewModels
{
    public class GameDetailViewModel
    {
        public Game Game { get; set; } = new Game();
        public List<Game> RelatedGames { get; set; } = new List<Game>();
        public string CurrentLanguage { get; set; } = "nl";
        public bool IsDemo { get; set; } = true;
    }
}