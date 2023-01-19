using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MinLength(4), MaxLength(50)]
        public string Name { get; set; }

        public string AvatarURL { get; set; }

        [Required]
        public float MoneyOnAccount { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastLogInDate { get; set; }

        [Required]
        public ICollection<User> Friends { get; set; }

        [Required]
        public ICollection<GameStats> GamesStats { get; set; }

    }
}
