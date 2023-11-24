using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
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

        [HttpGet("GetReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
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
                reviewValidation.Validate(newReview, dbcontext.Review, ModelState);

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
        public async Task<ActionResult<Review>> PutReview(int id, [FromBody] Review newReview)
        {
            try
            {
                reviewValidation.Validate(newReview, dbcontext.Review, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var reviewFromDB = await dbcontext.Review.FindAsync(id);

                if (reviewFromDB == null)
                    return NoContent();

                reviewFromDB.Text = newReview.Text;
                reviewFromDB.IsPositive = newReview.IsPositive;
                reviewFromDB.CreationDate = newReview.CreationDate;
                reviewFromDB.LastEditDate = newReview.LastEditDate;
                reviewFromDB.GameID = newReview.GameID;
                reviewFromDB.Game = newReview.Game;
                reviewFromDB.UserID = newReview.UserID;
                reviewFromDB.User = newReview.User;

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
