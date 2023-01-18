using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MinLength(4), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        //[ForeignKey("Developer")]
        public virtual Developer Developer { get; set; }

        public uint AchievementsCount { get; set; }

        [Required]
        //[ForeignKey("Review")]
        public ICollection<Review> Reviews { get; set; }
    }
}
