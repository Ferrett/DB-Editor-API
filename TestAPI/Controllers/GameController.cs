using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.S3Bucket.Game;
using WebAPI.Services.Validation.GameValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Game")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IGameValidation gameValidation;
        private readonly IS3Bucket bucket;
        private readonly IConfiguration configuration;
        public GameController(ApplicationDbContext context, IS3Bucket _bucket, IGameValidation _gameValidation, IConfiguration _configuration)
        {
            dbcontext = context;
            gameValidation = _gameValidation;
            bucket = _bucket;
            configuration = _configuration;
        }

        [HttpGet("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            try
            {
                return Ok(await dbcontext.Game.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGame/{id:int}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            try
            {
                var game = await dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostGame")]
        public async Task<ActionResult<Game>> PostGame([FromBody] Game newGame)
        {
            try
            {
                gameValidation.Validate(newGame, dbcontext.Game.ToList(), ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.Game.AddAsync(newGame);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostGame), new { id = newGame.ID }, newGame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGame/{id:int}")]
        public async Task<ActionResult<Game>> PutGame(int id, [FromBody] Game newGame)
        {
            try
            {
                gameValidation.Validate(newGame, dbcontext.Game.ToList(), ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gameFromDb = await dbcontext.Game.FindAsync(id);

                if (gameFromDb == null)
                    return NoContent();

                gameFromDb.Name = newGame.Name;
                gameFromDb.LogoURL = newGame.LogoURL;
                gameFromDb.Price = newGame.Price;
                gameFromDb.PublishDate= newGame.PublishDate;
                gameFromDb.AchievementsCount= newGame.AchievementsCount;
                gameFromDb.DeveloperID= newGame.DeveloperID;
                gameFromDb.Developer= newGame.Developer;
                gameFromDb.Reviews= newGame.Reviews;

                await dbcontext.SaveChangesAsync();

                return Ok(gameFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutLogo/{id:int}")]
        public async Task<ActionResult<Game>> PutLogo(int id, IFormFile? logo = null)
        {
            try
            {
                GameLogoUpload gameLogoUpload = new GameLogoUpload(configuration);

                var game = await dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                if (logo == null)
                {
                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, guid).Wait();
                    bucket.DeleteObject(game.LogoURL!).Wait();

                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{guid}";
                }

                await dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteGame/{id:int}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            try
            {
                var game = await dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                dbcontext.Game.Remove(game);
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
