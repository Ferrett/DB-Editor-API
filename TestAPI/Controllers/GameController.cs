using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        [HttpGet("GetAllGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
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
                gameValidation.Validate(newGame, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                GameLogoUpload gameLogoUpload = new GameLogoUpload(configuration);
                newGame.LogoURL = $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}";

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
        public async Task<ActionResult<Game>> PutGame(int id, [FromBody] Game updatedGame)
        {
            try
            {
                updatedGame.ID = id;
                gameValidation.Validate(updatedGame, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gameFromDb = await dbcontext.Game.FindAsync(id);

                if (gameFromDb == null)
                    return NoContent();

                gameFromDb.Name = updatedGame.Name;
                gameFromDb.Price = updatedGame.Price;
                gameFromDb.PublishDate = updatedGame.PublishDate;
                gameFromDb.AchievementsCount = updatedGame.AchievementsCount;
                gameFromDb.DeveloperID = updatedGame.DeveloperID;
                gameFromDb.Developer = updatedGame.Developer;

                await dbcontext.SaveChangesAsync();

                return Ok(gameFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameLogo/{id:int}")]
        public async Task<ActionResult<Game>> PutGameLogo(int id, IFormFile? logo = null)
        {
            try
            {
                GameLogoUpload gameLogoUpload = new GameLogoUpload(configuration);

                var game = await dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                if (game.LogoURL != $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}")
                    await gameLogoUpload.DeleteObject(game.LogoURL!);

                if (logo == null)
                {
                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}";
                }
                else
                {
                    Guid newLogoGuid = Guid.NewGuid();

                    await gameLogoUpload.AddObject(logo, newLogoGuid);

                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{newLogoGuid}";
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
