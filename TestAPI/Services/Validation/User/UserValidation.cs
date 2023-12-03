using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserValidation
{
    public class UserValidation : IUserValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public UserValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }
        public void Validate(User user, ModelStateDictionary modelState)
        {
            if (!dbcontext.User.Any(x => x.ID == user.ID) &&  dbcontext.User.Any(x => x.Login.ToLower() == user.Login.ToLower()))
                modelState.AddModelError("LoginAlreadyExists", $"User with login \"{user.Login}\" already exists");
        }
    }
}
