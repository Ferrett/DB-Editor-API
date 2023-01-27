using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Developer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50), MinLength(4)]
        public string Name { get; set; } = null!;

        [Required]
        public string LogoURL { get; set; } = null!;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDate { get; set; }

        public ICollection<Game>? PublishedGames { get; set; }
    }
}
