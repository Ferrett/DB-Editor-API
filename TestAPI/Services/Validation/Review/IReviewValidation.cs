using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Services.Validation.ReviewValidation
{
    public interface IReviewValidation
    {
        void Validate(Review newReview, List<Review> reviews, ModelStateDictionary modelState);
    }
}
