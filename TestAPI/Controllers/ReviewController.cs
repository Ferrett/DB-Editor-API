using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/Review")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IReviewValidation reviewValidation;
        public ReviewController(ApplicationDbContext context, IReviewValidation _reviewValidation)
        {
            dbcontext = context;
            reviewValidation = _reviewValidation;
        }

        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            try
            {
                return Ok(await dbcontext.Review.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReview/{id:int}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                var review = await dbcontext.Review.FindAsync(id);

                if (review == null)
                    return NoContent();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostReview")]
        public async Task<ActionResult<Review>> PostReview([FromBody] Review newReview)
        {
            try
            {
                reviewValidation.Validate(newReview, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.Review.AddAsync(newReview);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostReview), new { id = newReview.ID }, newReview);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutReview/{id:int}")]
        public async Task<ActionResult<Review>> PutReview(int id, [FromBody] Review updatedReview)
        {
            try
            {
                updatedReview.ID = id;
                reviewValidation.Validate(updatedReview, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var reviewFromDB = await dbcontext.Review.FindAsync(id);

                if (reviewFromDB == null)
                    return NoContent();

                reviewFromDB.Text = updatedReview.Text;
                reviewFromDB.IsPositive = updatedReview.IsPositive;
                reviewFromDB.CreationDate = updatedReview.CreationDate;
                reviewFromDB.LastEditDate = updatedReview.LastEditDate;
                reviewFromDB.GameID = updatedReview.GameID;
                reviewFromDB.Game = updatedReview.Game;
                reviewFromDB.UserID = updatedReview.UserID;
                reviewFromDB.User = updatedReview.User;

                await dbcontext.SaveChangesAsync();

                return Ok(reviewFromDB);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteReview/{id:int}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            try
            {
                var review = await dbcontext.Review.FindAsync(id);

                if (review == null)
                    return NoContent();

                dbcontext.Review.Remove(review);
                await dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
