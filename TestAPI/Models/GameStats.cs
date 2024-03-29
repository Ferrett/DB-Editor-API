﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class GameStats
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(0.0, float.MaxValue)]
        public float HoursPlayed { get; set; }

        [Range(0, 9999)]
        public int AchievementsGotten { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int GameID { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Game? Game { get; set; } 
    }
}
