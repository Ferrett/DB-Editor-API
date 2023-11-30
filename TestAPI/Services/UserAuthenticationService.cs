using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Models.ServiceModels;
using WebAPI.Services.S3Bucket.User;

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

        public Task<User?> FindUserByLogin(string login)
        {
            return dbcontext.User.FirstOrDefaultAsync(x => x.Login == login);
        }

        public bool IsPasswordCorrect(User user, string password)
        {
            return user.Password == password ? true : false;
        }
    }
}
