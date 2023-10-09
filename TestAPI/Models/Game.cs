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
        [MinLength(5, ErrorMessage = "The Link is incorrect")]
        public string LogoURL { get; set; } = null!;

        [Required]
        [Range(0.0, 1000)]
        public float Price { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Range(0.0, 1000)]
        public int AchievementsCount { get; set; }

        [Required]
        public int DeveloperID { get; set; }
        public Developer Developer { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
    }
}
