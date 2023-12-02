using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;
using WebAPI.Models.ServiceModels;

namespace WebAPI.Services
{
    public interface IUserAuthenticationService
    {
        public User RegistrationModelToUser(RegistrationModel userRegister);
        public void LoginAttempt(LoginModel loginModel, ModelStateDictionary modelState);
    }
}
