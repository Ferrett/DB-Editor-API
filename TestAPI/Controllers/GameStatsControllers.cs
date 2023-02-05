using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/GameStats")]
    public class GameStatsControllers : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().GamesStats);

                return Ok(new ApplicationContext().GamesStats.ToList());
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

                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
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

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    db.GamesStats.Remove(gameStats);
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
        public IActionResult Post(int userID, int gameID)
        {
            try
            {
                Validation.GameSatsExists(userID, gameID);
                Validation.ValidateUserID(userID);
                Validation.ValidateGameID(gameID);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = new GameStats
                    {
                        UserID = userID,
                        GameID = gameID,
                        HoursPlayed= 0,
                        AchievementsGot = 0,
                        PurchasehDate = DateTime.UtcNow,
                    };

                    db.GamesStats.Add(gameStats);
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

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
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

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
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


        [HttpPut("PutGameLaunchDate/{id:int}")]
        public IActionResult PutGameLaunchDate(int id)
        {
            try
            {
                Validation.ValidateGameStatsID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    GameStats gameStats = db.GamesStats.Where(x => x.ID == id).First();
                    gameStats.LastLaunchDate = DateTime.UtcNow;
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
