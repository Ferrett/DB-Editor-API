using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameValidation
{
    public interface IGameValidation
    {
        void Validate(Game newGame, ModelStateDictionary modelState);
    }
}
