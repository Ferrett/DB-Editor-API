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
        [MinLength(5, ErrorMessage = "The Link is incorrect")]
        public string LogoURL { get; set; } = null!;

        [Required]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore]
        public ICollection<Game> PublishedGames { get; set; } = new List<Game>();
    }
}
