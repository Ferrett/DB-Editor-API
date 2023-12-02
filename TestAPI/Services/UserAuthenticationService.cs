using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Models.ServiceModels;
using WebAPI.Services.S3Bucket.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbcontext;
        public UserAuthenticationService(IConfiguration _configuration, ApplicationDbContext _dbcontext)
        {
            configuration = _configuration;
            dbcontext = _dbcontext;
        }

        public User RegistrationModelToUser(RegistrationModel userRegister)
        {
            UserProfilePictureUpload userPfpUpload = new UserProfilePictureUpload(configuration);
            return new User()
            {
                CreationDate = DateTime.UtcNow,
                Email = userRegister.Email,
                Password = userRegister.Password,
                Nickname = userRegister.Nickname,
                Login = userRegister.Login,
                ProfilePictureURL = $"{userPfpUpload.BucketUrl}{userPfpUpload.Placeholder}",
            };
        }

        public void LoginAttempt(LoginModel loginModel, ModelStateDictionary modelState)
        {
            User? user = dbcontext.User.FirstOrDefault(x => x.Login == loginModel.Login);

            if (user == null)
                modelState.AddModelError("UserNotFound", $"User with login \"{loginModel.Login}\" not found");
            else if (user.Password != loginModel.Password)
                modelState.AddModelError("PasswordIncorrect", "Incorrect password");

        }
    }
}
