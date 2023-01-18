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
        //[ForeignKey("User")]
        public virtual User Owner { get; set; }

        [Required]
        //[ForeignKey("Game")]
        public virtual Game Game { get; set; }

        [Required]
        public float HoursPlayed { get; set; }

        public uint AchievementsGot { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PurchasehDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastLaunchDate { get; set; }

        //[ForeignKey("Review")]
        public virtual Review Review { get; set; }
    }
}
