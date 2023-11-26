using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserValidation
{
    public interface IUserValidation
    {
        void ValidateNewUser(User newUser,  ModelStateDictionary modelState);
    }
}
