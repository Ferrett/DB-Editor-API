using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Review")]
    public class ReviewController : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationDbContext().Review);

                return Ok(new ApplicationDbContext().Review.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                Validation.ValidateReviewID(id);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
                    return Ok(rev);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Validation.ValidateReviewID(id);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
                    db.Review.Remove(rev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Post/{isPostive:bool}/{gameID:int}/{userID:int}")]
        public IActionResult Post(bool isPostive, int userID, int gameID, string? text = null)
        {
            try
            {
                Validation.IsReviewExists(userID, gameID);
                Validation.ValidateUserID(userID);
                Validation.ValidateGameID(gameID);
                Validation.ValidateReviewText(text);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = new Review
                    {
                        Text = text == null? null:text,
                        IsPositive = isPostive,
                        GameID = gameID,
                        UserID = userID,
                        CreationDate = DateTime.UtcNow,
                        LastEditDate= DateTime.UtcNow,
                    };

                    db.Review.Add(rev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUser/{id:int}/{userID:int}")]
        public IActionResult PutUser(int id, int userID)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateUserID(userID);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
                    rev.UserID = userID;
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGame/{id:int}/{gameID:int}")]
        public IActionResult PutGame(int id, int gameID)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateGameID(gameID);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
                    rev.GameID = gameID;
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutText/{id:int}")]
        public IActionResult PutText(int id, string? text=null)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateReviewText(text);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
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

        [HttpPut("PutApproval/{id:int}/{isPositive:bool}")]
        public IActionResult PutApproval(int id, bool isPositive)
        {
            try
            {
                Validation.ValidateReviewID(id);
                Validation.ValidateReviewApproval(id, isPositive);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Review rev = db.Review.Where(x => x.ID == id).First();
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
