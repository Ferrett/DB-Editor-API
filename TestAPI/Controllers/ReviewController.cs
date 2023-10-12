using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Review")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public ReviewController(ApplicationDbContext context)
        {
            dbcontext = context;
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
        public async Task<ActionResult<Review>> PostReview([FromBody] Review review)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.Review.AddAsync(review);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostReview), new { id = review.ID }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutReview/{id:int}")]
        public async Task<ActionResult<Review>> PutReview(int id, [FromBody] Review review)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != review.ID)
                    return BadRequest();

                var reviewFromDB = await dbcontext.Review.FindAsync(id);

                if (reviewFromDB == null)
                    return NoContent();

                reviewFromDB.Text = review.Text;
                reviewFromDB.IsPositive = review.IsPositive;
                reviewFromDB.CreationDate = review.CreationDate;
                reviewFromDB.LastEditDate = review.LastEditDate;
                reviewFromDB.GameID = review.GameID;
                reviewFromDB.Game = review.Game;
                reviewFromDB.UserID = review.UserID;
                reviewFromDB.User = review.User;

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
