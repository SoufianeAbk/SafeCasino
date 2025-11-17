using SafeCasino.Models;

namespace SafeCasino.Services
{
    public interface IGameApiService
    {
        Task<ApiGameResponse> GetGamesAsync(FilterOptions filters);
        Task<Game?> GetGameByIdAsync(int id);
        Task<List<Game>> GetPopularGamesAsync(int count = 10);
        Task<List<Game>> GetNewGamesAsync(int count = 10);
        Task<List<Game>> GetJackpotGamesAsync(int count = 10);
        Task<List<Game>> GetRelatedGamesAsync(int gameId, int count = 5);
        Task<List<string>> GetProvidersAsync();
        Task<List<GameCategory>> GetCategoriesAsync();
    }
}