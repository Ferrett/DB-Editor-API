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
                Validation.ValidateList(new ApplicationContext().Games);

                using (ApplicationContext db = new ApplicationContext())
                {

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
                Validation.ValidateGameID(id);

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
                Validation.ValidateGameID(id);

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
        public IActionResult PostGame(string name, float price, int devID, int achCount = 0)
        {
            try
            {
                Validation.ValidateGameName(name);
                Validation.ValidateGamePrice(price);
                Validation.ValidateDeveloperID(devID);
                Validation.ValidateAchievementsCount(achCount);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = new Game
                    {
                        Name = name,
                        LogoURL = $"{S3Bucket.GameBucketUrl}{S3Bucket.DefaultLogoName}",
                        Price = price,
                        Developer = db.Developers.Where(x => x.ID == devID).First(),
                        AchievementsCount = achCount,
                        PublishDate = DateTime.UtcNow
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
                Validation.ValidateGameID(id);
                Validation.ValidateAchievementsCount(achCount);

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
                Validation.ValidateGameID(id);
                Validation.ValidateGamePrice(price);

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
                Validation.ValidateGameID(id);
                Validation.ValidateGameName(name);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Validation.ValidateGameID(id);

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
                Validation.ValidateGameID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.GameBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(db.Developers.Where(x => x.ID == id).First().LogoURL, S3Bucket.GameBucketPath).Wait();

                    Game game = db.Games.Where(x => x.ID == id).First();
                    game.LogoURL = $"{S3Bucket.GameBucketUrl}{guid}";
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
