using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;

namespace WebAPI.Services.Validation.ReviewValidation
{
    public class ReviewValidation : IReviewValidation
    {
        private readonly ApplicationDbContext dbcontext;
        public ReviewValidation(ApplicationDbContext context)
        {
            dbcontext = context;
        }
        public void Validate(Review newReview, ModelStateDictionary modelState)
        {
            if (dbcontext.Review.Any(x => x.UserID == newReview.UserID && x.GameID == newReview.GameID))
                modelState.AddModelError("AlreadyExists", "This review already exists");

            if (!dbcontext.User.Any(x => x.ID == newReview.UserID))
                modelState.AddModelError("UserNotExists", $"User with ID \"{newReview.UserID}\" not exists");

            if (!dbcontext.Game.Any(x => x.ID == newReview.GameID))
                modelState.AddModelError("GameNotExists", $"Game with ID \"{newReview.GameID}\" not exists");
        }
    }
}
