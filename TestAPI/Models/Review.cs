using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(1000)]
        public string Text { get; set; }

        [Required]
        public bool IsPositive { get; set; }

        [Required]
        //[ForeignKey("Game")]
        public virtual Game Game { get; set; }

        [Required]
        //[ForeignKey("User")]
        public virtual User Author { get; set; }
    }
}
