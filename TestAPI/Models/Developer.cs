using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50), MinLength(4)]
        public string Name { get; set; } = null!;

        [Required]
        public string LogoURL { get; set; } = null!;

        [Required]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore]
        public virtual List<Game> PublishedGames { get; set; } = new List<Game>();
    }
}
