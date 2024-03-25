using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameValidation
{
    public class GameValidation : IGameValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public GameValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public void Validate(Game game, ModelStateDictionary modelState)
        {
            if (dbcontext.Game.Any(x => (x.Title.ToLower() == game.Title.ToLower()) && (x.ID != game.ID)))
                modelState.AddModelError("AlreadyExists", $"Game with name \"{game.Title}\" already exists");

            if (!dbcontext.Developer.Any(x => x.ID == game.DeveloperID))
                modelState.AddModelError("DeveloperNotExists", $"Developer with ID \"{game.DeveloperID}\" not exists");
        }
    }
}
