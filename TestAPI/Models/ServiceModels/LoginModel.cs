using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ServiceModels
{
    public class LoginModel
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
