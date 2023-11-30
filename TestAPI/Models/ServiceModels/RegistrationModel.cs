using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ServiceModels
{
    public class RegistrationModel
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public string? Email { get; set; }
    }
}
