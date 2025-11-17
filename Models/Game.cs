namespace SafeCasino.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string GameUrl { get; set; } = string.Empty;
        public GameCategory Category { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
        public decimal RTP { get; set; }
        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }
        public bool HasJackpot { get; set; }
        public DateTime AddedDate { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}