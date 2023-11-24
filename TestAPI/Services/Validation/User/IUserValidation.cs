using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserValidation
{
    public interface IUserValidation
    {
        void Validate(User newUser, DbSet<User> users, ModelStateDictionary modelState);
    }
}
