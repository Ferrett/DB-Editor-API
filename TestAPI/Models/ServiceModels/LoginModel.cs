using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ServiceModels
{
    public class LoginModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 7)]
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
