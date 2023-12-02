using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ServiceModels
{
    public class RegistrationModel
    {
        [Required]
        [StringLength(99, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Nickname { get; set; } = null!;

        [MaxLength(256)]
        public string? Email { get; set; }
    }
}
