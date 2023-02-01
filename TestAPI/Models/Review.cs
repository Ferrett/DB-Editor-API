using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public Game? Game { get; set; }

        [Required]
        public User? Author { get; set; }
    }
}
