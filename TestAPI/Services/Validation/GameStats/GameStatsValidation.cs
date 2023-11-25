using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameStatsValidation
{
    public class GameStatsValidation : IGameStatsValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public GameStatsValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }
        public void Validate(GameStats newGameStats, ModelStateDictionary modelState)
        {
            if (dbcontext.GameStats.Any(x => x.UserID == newGameStats.UserID && x.GameID == newGameStats.GameID))
                modelState.AddModelError("AlreadyExists", "This game statistics already exists");

            if (!dbcontext.User.Any(x => x.ID == newGameStats.UserID))
                modelState.AddModelError("UserNotExists", $"User with ID \"{newGameStats.UserID}\" not exists");

            if (!dbcontext.Game.Any(x => x.ID == newGameStats.GameID))
                modelState.AddModelError("GameNotExists", $"Game with ID \"{newGameStats.GameID}\" not exists");
        }
    }
}
