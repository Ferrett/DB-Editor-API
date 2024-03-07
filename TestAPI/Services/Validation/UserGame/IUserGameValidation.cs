using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;

namespace WebAPI.Services.Validation.UserGameValidation
{
    public interface IUserGameValidation
    {
        void Validate(UserGame userGame, ModelStateDictionary modelState);
    }
}
