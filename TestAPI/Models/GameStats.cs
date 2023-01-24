using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class GameStats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public virtual User Owner { get; set; }

        [Required]
        public virtual Game Game { get; set; }

        [Required]
        public float HoursPlayed { get; set; }

        public int AchievementsGot { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PurchasehDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastLaunchDate { get; set; }

        public virtual Review Review { get; set; }
    }
}
