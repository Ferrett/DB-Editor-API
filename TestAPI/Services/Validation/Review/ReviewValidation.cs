using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;

namespace WebAPI.Services.Validation.ReviewValidation
{
    public class ReviewValidation : IReviewValidation
    {
        public void Validate(Review newReview, DbSet<Review> reviews, ModelStateDictionary modelState)
        {
            if (reviews.Where(x => x.UserID == newReview.UserID && x.GameID == newReview.GameID).Any())
                modelState.AddModelError("AlreadyExists", "This review already exists");
        }
    }
}
