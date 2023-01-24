using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
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
                Validation.ValidateList(new ApplicationContext().Reviews);

                return Ok(new ApplicationContext().Reviews.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReview/{id:int}")]
        public IActionResult GetReview(int id)
        {
            try
            {
                Validation.ValidateReviewID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    return Ok(rev);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteReview/{id:int}")]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                Validation.ValidateReviewID(id);

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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostReview/{isPostive:bool}/{gameID:int}/{authorID:int}")]
        public IActionResult PostReview(bool isPostive, int authorID, int gameID, string? text = null)
        {
            try
            {
                Validation.ValidateUserID(authorID);
                Validation.ValidateGameID(authorID);
                Validation.ValidateReviewText(text);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = new Review
                    {
                        Text = text == null? DBNull.Value.ToString():text,
                        IsPositive = isPostive,
                        Game = db.Games.Where(x => x.ID == gameID).First(),
                        Author = db.Users.Where(x => x.ID == authorID).First(),
                        CreationDate = DateTime.UtcNow,
                        LastEditDate= DateTime.UtcNow,
                    };

                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PutReviewText/{id:int}")]
        public IActionResult PutReviewText(int id, string? text=null)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateReviewText(text);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    rev.Text = text == null ? DBNull.Value.ToString() : text;
                    rev.LastEditDate = DateTime.UtcNow;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PutReviewApproval/{id:int}/{isPositive:bool}")]
        public IActionResult PutReviewApproval(int id, bool isPositive)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateReviewApproval(id, isPositive);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Review rev = db.Reviews.Where(x => x.ID == id).First();
                    rev.IsPositive = isPositive;
                    rev.LastEditDate = DateTime.UtcNow;
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
