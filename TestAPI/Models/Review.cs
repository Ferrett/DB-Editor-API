using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(9999)]
        public string? Text { get; set; }

        [Required]
        public bool IsPositive { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime LastEditDate { get; set; }

        [Required]
        public int GameID { get; set; }

        [Required]
        public int UserID { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; }

        [JsonIgnore]
        public User? User { get; set; } 
    }
}
