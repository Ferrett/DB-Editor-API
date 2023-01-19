using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ReviewControllers : Controller
    {
        [HttpGet("GetAllReviews")]
        public IActionResult GetAllReviews()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.Reviews.ToArray().Length == 0)
                        throw new ArgumentNullException("Sequence has no elements");

                    return Ok(JsonSerializer.Serialize(db.Reviews));
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetReviewByID/{id:int}")]
        public IActionResult GetReviewByID(int id)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    return Ok(rev);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteReviewByID/{id:int}")]
        public IActionResult DeleteReviewByID(int id)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    db.Reviews.Remove(rev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("PostNewReview/{isPostive:bool}/{text}/{gameID:int}/{authorID:int}")]
        public IActionResult PostNewReview(bool isPostive, string text, int gameID, int authorID)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = new Review
                    {
                        Text = text,
                        IsPositive = isPostive,
                        Game = db.Games.Where(x => x.ID == gameID).First(),
                        Author = db.Users.Where(x => x.ID == authorID).First(),
                        CreationDate = DateTime.Now
                    };

                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("PutReviewText/{id:int}/{text}")]
        public IActionResult PutReviewText(int id, string text)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    rev.Text = text;
                    rev.LastEditDate =DateTime.Now;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("PutReviewApproval/{id:int}/{isPositive:bool}")]
        public IActionResult PutReviewApproval(int id, bool isPositive)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    rev.IsPositive = isPositive;
                    rev.LastEditDate = DateTime.Now;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
