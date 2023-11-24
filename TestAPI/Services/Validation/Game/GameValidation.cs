using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.GameValidation
{
    public class GameValidation : IGameValidation
    {
        public void Validate(Game newGame, DbSet<Game> games, ModelStateDictionary modelState)
        {
            if (IsAllLettersOrDigits(newGame.Name) == false)
                modelState.AddModelError("NameLettersOrDigits", $"Game name can contain only latin letters or digits");

            if (games.Where(x => x.Name.ToLower() == newGame.Name.ToLower()).Any())
                modelState.AddModelError("AlreadyExists", $"Game with name {newGame.Name} already exists");
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
