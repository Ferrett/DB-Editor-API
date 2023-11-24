using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameStatsValidation
{
    public class GameStatsValidation : IGameStatsValidation
    {
        public void Validate(GameStats newGameStats, List<GameStats> gamesStats, ModelStateDictionary modelState)
        {
            if (gamesStats.Where(x => x.UserID == newGameStats.UserID && x.GameID == newGameStats.GameID).Any())
                modelState.AddModelError("AlreadyExists", "This games statistics already exists");
        }
    }
}
