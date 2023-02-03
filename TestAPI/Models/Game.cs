using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(25),MinLength(4)]
        public string Name { get; set; } = null!;

        [Required]
        public string LogoURL { get; set; } = null!;

        [Required]
        public float Price { get; set; }

        [Required]
        public int DeveloperID { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public int AchievementsCount { get; set; }

        [JsonIgnore]
        public virtual List<Review>? Reviews { get; set; } = new List<Review>();
    }
}
