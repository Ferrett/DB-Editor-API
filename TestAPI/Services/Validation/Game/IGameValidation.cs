using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameValidation
{
    public interface IGameValidation
    {
        void Validate(Game game, ModelStateDictionary modelState);
    }
}
