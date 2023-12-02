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

            if (IsAllLettersOrDigits(user.Login) == false)
                modelState.AddModelError("LoginLettersOrDigits", "Login can contain only latin letters or digits");

            if (user.Email != null && new EmailAddressAttribute().IsValid(user.Email) == false)
                modelState.AddModelError("EmailIsNotValid", $"Email \"{user.Email}\" is not valid");
        }

        public bool IsAllLettersOrDigits(string str)
        {
            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') == false && (c >= 'A' && c <= 'Z') == false && (c >= '0' && c <= '9') == false)
                    return false;
            }
            return true;
        }
    }
}
