using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class GameStats
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public  User Owner { get; set; } = null!;

        [Required]
        public  Game Game { get; set; } = null!;

        [Required]
        public float HoursPlayed { get; set; }

        public int AchievementsGot { get; set; }

        [Required]
        public DateTime PurchasehDate { get; set; }

        public DateTime LastLaunchDate { get; set; }

        public Review? Review { get; set; }
    }
}
