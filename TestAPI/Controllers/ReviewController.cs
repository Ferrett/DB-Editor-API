using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.ReviewValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/Review")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IReviewValidation _reviewValidation;
        public ReviewController(ApplicationDbContext dbcontext, IReviewValidation reviewValidation)
        {
            _dbcontext = dbcontext;
            _reviewValidation = reviewValidation;
        }

        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            try
            {
                return Ok(await _dbcontext.Review.ToListAsync());
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
                var review = await _dbcontext.Review.FindAsync(id);

                if (review == null)
                    return NoContent();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReviewsByGameID/{gameID:int}")]
        public async Task<ActionResult<Review>> GetReviewsByGameID(int gameID)
        {
            try
            {
                var reviews = await _dbcontext.Review.Where(x => x.GameID == gameID).ToListAsync();
                
                if (reviews == null)
                    return NoContent();

                return Ok(reviews);
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
                _reviewValidation.Validate(newReview, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _dbcontext.Review.AddAsync(newReview);
                await _dbcontext.SaveChangesAsync();

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
                _reviewValidation.Validate(updatedReview, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var reviewFromDB = await _dbcontext.Review.FindAsync(id);

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

                await _dbcontext.SaveChangesAsync();

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
                var review = await _dbcontext.Review.FindAsync(id);

                if (review == null)
                    return NoContent();

                _dbcontext.Review.Remove(review);
                await _dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
