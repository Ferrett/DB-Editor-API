using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameStatsValidation
{
    public interface IGameStatsValidation
    {
        void Validate(GameStats gameStats, ModelStateDictionary modelState);
    }
}
