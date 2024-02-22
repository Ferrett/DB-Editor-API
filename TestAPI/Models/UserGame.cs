using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class UserGame
    {
        public int UserID { get; set; }
        public int GameID { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; }
    }
}
