using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Game")]
    public class GameControllers : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().Games);

                return Ok(new ApplicationContext().Games.ToList());
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

        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
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

        [HttpPost("Post/{name}/{price:float}/{devID:int}")]
        public IActionResult Post(string name, float price, int devID, int achCount = 0)
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
                        DeveloperID = devID,
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

        [HttpPut("PutPrice/{id:int}/{price:float}")]
        public IActionResult PutPrice(int id, float price)
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

        [HttpPut("PutName/{id:int}/{name}")]
        public IActionResult PutName(int id, string name)
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

        [HttpPut("PutLogo/{id:int}/{logo}")]
        public IActionResult PutLogo(int id, IFormFile logo)
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
