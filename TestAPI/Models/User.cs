﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace WebAPI.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 7)]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Nickname { get; set; } = null!;

        public string? ProfilePictureURL { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string? Email { get; set; }

        [JsonIgnore]
        public Developer? Developer {get;set;}

        [JsonIgnore]
        public ICollection<GameStats>? GamesStats { get; set; } = new List<GameStats>();
    }
}