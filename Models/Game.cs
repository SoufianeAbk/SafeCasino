using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeCasino.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Provider { get; set; } = string.Empty;

        [StringLength(500)]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [StringLength(500)]
        public string GameUrl { get; set; } = string.Empty;

        [Required]
        public GameCategory Category { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinBet { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MaxBet { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal RTP { get; set; }

        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }
        public bool HasJackpot { get; set; }

        public DateTime AddedDate { get; set; }

        [NotMapped]
        public List<string> Tags { get; set; } = new List<string>();
    }

    public enum GameCategory
    {
        Slots,
        Roulette,
        Blackjack,
        LiveCasino,
        Jackpot
    }
}