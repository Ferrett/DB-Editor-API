using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web.Http.Results;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    public class GameControllers : Controller
    {
        [HttpGet("GetAllGames")]
        public IActionResult GetAllGames()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.Games.ToArray().Length == 0)
                        throw new ArgumentNullException("Sequence has no elements");

                    return Ok(db.Games.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGame/{id:int}")]
        public IActionResult GetGame(int id)
        {
            try
            {
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

        [HttpDelete("DeleteGame/{id:int}")]
        public IActionResult DeleteGame(int id)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    db.Games.Remove(game);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostGame/{name}/{price:float}/{devID:int}")]
        public IActionResult PostGame(string name, IFormFile logo, float price, int devID, int achCount=0)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = new Game
                    {
                        Name = name,
                        LogoURL= logo.FileName,
                        Price = price,
                        Developer = db.Developers.Where(x => x.ID == devID).First(),
                        AchievementsCount = achCount,
                        PublishDate= DateTime.Now
                    };

                    db.Games.Add(game);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutAchievementsCount/{id:int}/{achCount:int}")]
        public IActionResult PutAchievementsCount(int id, int achCount)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    game.AchievementsCount = achCount;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGamePrice/{id:int}/{price:float}")]
        public IActionResult PutGamePrice(int id, float price)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    game.Price = price;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameName/{id:int}/{name}")]
        public IActionResult PutGameName(int id, string name)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    game.Name = name;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameLogo/{id:int}/{logo}")]
        public IActionResult PutGameLogo(int id, IFormFile logo)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = db.Games.Where(x => x.ID == id).First();
                    game.Name = logo.FileName;
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
