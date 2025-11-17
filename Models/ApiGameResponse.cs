namespace SafeCasino.Models
{
    public class ApiGameResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    }
}