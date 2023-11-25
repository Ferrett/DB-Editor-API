using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserValidation
{
    public class UserValidation : IUserValidation
    {
        public void Validate(User newUser, List<User> users, ModelStateDictionary modelState)
        {
            if (IsAllLettersOrDigits(newUser.Login) == false)
                modelState.AddModelError("LoginLettersOrDigits", "Login can contain only latin letters or digits");

            if (users.Where(x => x.Login.ToLower() == newUser.Login.ToLower()).Any())
                modelState.AddModelError("LoginAlreadyExists", $"User with login {newUser.Login} already exists");

            if (newUser.Email != null && new EmailAddressAttribute().IsValid(newUser.Email) == false)
                modelState.AddModelError("EmailAlreadyExists", $"Email {newUser.Email} is not valid");
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
