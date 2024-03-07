using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public void Validate(GameStats gameStats, ModelStateDictionary modelState)
        {
            if (dbcontext.GameStats.Any(x => (x.UserID == gameStats.UserID && x.GameID == gameStats.GameID) && (x.ID != gameStats.ID)))
                modelState.AddModelError("AlreadyExists", "This game statistics already exists");

            if (!dbcontext.User.Any(x => x.ID == gameStats.UserID))
                modelState.AddModelError("UserNotExists", $"User with ID \"{gameStats.UserID}\" not exists");

            if (!dbcontext.Game.Any(x => x.ID == gameStats.GameID))
                modelState.AddModelError("GameNotExists", $"Game with ID \"{gameStats.GameID}\" not exists");

            if (dbcontext.Game.Where(x => x.ID == gameStats.GameID).First().AchievementsAmount < gameStats.AchievementsGotten)
                modelState.AddModelError("AchievementsOverflow", $"New achievements amount can't be bigger than Game achievements amount");
        }
    }
}
