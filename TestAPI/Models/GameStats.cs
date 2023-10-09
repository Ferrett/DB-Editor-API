using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class GameStats
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public int GameID { get; set; }
        public Game Game { get; set; } = null!;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public float HoursPlayed { get; set; }

        [Range(0.0, 1000)]
        public int AchievementsGot { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
