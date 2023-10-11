using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/GameStats")]
    public class GameStatsController : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationDbContext().GameStats);

                return Ok(new ApplicationDbContext().GameStats.ToList());
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
                Validation.ValidateGameStatsID(id);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Game game = db.Game.Where(x => x.ID == id).First();
                    return Ok(game);
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
                Validation.ValidateGameStatsID(id);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = db.GameStats.Where(x => x.ID == id).First();
                    db.GameStats.Remove(gameStats);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Post/{userID:int}/{gameID:int}")]
        public IActionResult Post(int userID, int gameID, int achGot=0,int hoursPlayed=0)
        {
            try
            {
                Validation.GameSatsExists(userID, gameID);
                Validation.ValidateUserID(userID);
                Validation.ValidateGameID(gameID);
                Validation.ValidateGottenAchievements(0, achGot);
                Validation.ValidateHoursPlayed(0,hoursPlayed);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = new GameStats
                    {
                        UserID = userID,
                        GameID = gameID,
                        HoursPlayed= hoursPlayed,
                        AchievementsGot = achGot,
                        PurchaseDate = DateTime.UtcNow,
                    };

                    db.GameStats.Add(gameStats);
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
                Validation.ValidateGameStatsID(id);
                Validation.ValidateGameID(gameID);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = db.GameStats.Where(x => x.ID == id).First();
                    gameStats.GameID = gameID;
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
                Validation.ValidateGameStatsID(id);
                Validation.ValidateUserID(userID);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = db.GameStats.Where(x => x.ID == id).First();
                    gameStats.UserID = userID;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGottenAchievements/{id:int}/{achGot:int}")]
        public IActionResult PutGottenAchievements(int id, int achGot)
        {
            try
            {
                Validation.ValidateGameStatsID(id);
                Validation.ValidateGottenAchievements(id, achGot);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = db.GameStats.Where(x => x.ID == id).First();
                    gameStats.AchievementsGot = achGot;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutHoursPlayed/{id:int}/{hoursPlayed:float}")]
        public IActionResult PutHoursPlayed(int id, float hoursPlayed)
        {
            try
            {
                Validation.ValidateGameStatsID(id);
                Validation.ValidateHoursPlayed(id, hoursPlayed);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    GameStats gameStats = db.GameStats.Where(x => x.ID == id).First();
                    gameStats.HoursPlayed = hoursPlayed;
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
