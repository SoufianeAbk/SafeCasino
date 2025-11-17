namespace SafeCasino.Models
{
    public class FilterOptions
    {
        public string? SearchTerm { get; set; }
        public GameCategory? Category { get; set; }
        public string? Provider { get; set; }
        public decimal? MinBet { get; set; }
        public decimal? MaxBet { get; set; }
        public bool? HasJackpot { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsPopular { get; set; }
        public string SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}