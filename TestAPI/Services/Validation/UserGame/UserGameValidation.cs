using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI.Logic;
using WebAPI.Models;


namespace WebAPI.Services.Validation.UserGameValidation
{
    public class UserGameValidation : IUserGameValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public UserGameValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public void Validate(UserGame userGame, ModelStateDictionary modelState)
        {
            if (!dbcontext.User.Any(x => x.ID == userGame.UserID))
                modelState.AddModelError("UserNotExists", $"User with ID \"{userGame.UserID}\" not exists");

            if (!dbcontext.Game.Any(x => x.ID == userGame.GameID))
                modelState.AddModelError("GameNotExists", $"Game with ID \"{userGame.GameID}\" not exists");

            if (dbcontext.UserGame.Any(x => x.GameID == userGame.GameID) && dbcontext.UserGame.Any(x => x.UserID == userGame.UserID))
                modelState.AddModelError("AlreadyExists", $"This pair already exists");
        }
    }
}
