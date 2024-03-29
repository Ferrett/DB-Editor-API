﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(99, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Nickname { get; set; } = null!;

        public string? ProfilePictureURL { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        [Range(0.0, float.MaxValue, ErrorMessage = "{0} must be greater than or equal to 0")]
        public float BalanceUSD { get; set; }

        [JsonIgnore]
        public ICollection<GameStats>? GamesStats { get; set; } = new List<GameStats>();

        [JsonIgnore]
        public ICollection<UserGame>? UserGames { get; set; } = new List<UserGame>();
    }
}