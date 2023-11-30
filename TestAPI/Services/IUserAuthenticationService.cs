using WebAPI.Models;
using WebAPI.Models.ServiceModels;

namespace WebAPI.Services
{
    public interface IUserAuthenticationService
    {
        public User RegistrationModelToUser(RegistrationModel userRegister);
        public Task<User?> FindUserByLogin(string login);
        public bool IsPasswordCorrect(User user, string password);
    }
}
