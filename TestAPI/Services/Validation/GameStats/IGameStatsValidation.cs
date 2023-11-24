using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameStatsValidation
{
    public interface IGameStatsValidation
    {
        void Validate(GameStats newGameStats, List<GameStats> gamesStats, ModelStateDictionary modelState);
    }
}
