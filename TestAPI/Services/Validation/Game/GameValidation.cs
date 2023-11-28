using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
            if (!dbcontext.Game.Any(x => x.ID == game.ID) && dbcontext.Game.Any(x => x.Name.ToLower() == game.Name.ToLower()))
                modelState.AddModelError("AlreadyExists", $"Game with name \"{game.Name}\" already exists");

            if (!dbcontext.Developer.Any(x => x.ID == game.DeveloperID))
                modelState.AddModelError("DeveloperNotExists", $"Developer with ID \"{game.DeveloperID}\" not exists");

            if (IsAllLettersOrDigits(game.Name) == false)
                modelState.AddModelError("NameLettersOrDigits", "Game name can contain only latin letters or digits");
        }


        public bool IsAllLettersOrDigits(string str)
        {
            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') == false && (c >= 'A' && c <= 'Z') == false && (c >= '0' && c <= '9') == false)
                    return false;
            }
            return true;
        }
    }
}
