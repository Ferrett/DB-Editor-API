using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAPI.Logic;

namespace WebAPI.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(256), MinLength(1)]
        public string Name { get; set; } = null!;

        public string? LogoURL { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public ICollection<Game>? PublishedGames { get; set; } = new List<Game>();
    }
}
