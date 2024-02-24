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
        private readonly ApplicationDbContext _dbcontext;
        private readonly IGameValidation _gameValidation;
        private readonly IS3Bucket _bucket;
        private readonly IConfiguration _configuration;
        public GameController(ApplicationDbContext dbcontext, IS3Bucket bucket, IGameValidation gameValidation, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _gameValidation = gameValidation;
            _bucket = bucket;
            _configuration = configuration;
        }

        [HttpGet("GetAllGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
        {
            try
            {
                return Ok(await _dbcontext.Game.ToListAsync());
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
                var game = await _dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGamesByTitle/{gameTitle}")]
        public async Task<ActionResult<Game>> GetGamesByTitle(string gameTitle)
        {
            try
            {
                var games = await _dbcontext.Game.Where(x=>x.Title.ToLower().Contains(gameTitle.ToLower())).ToListAsync();

                if (games == null)
                    return NoContent();

                return Ok(games);
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
                _gameValidation.Validate(newGame, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                GameLogoUpload gameLogoUpload = new GameLogoUpload(_configuration);
                newGame.LogoURL = $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}";

                await _dbcontext.Game.AddAsync(newGame);
                await _dbcontext.SaveChangesAsync();

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
                _gameValidation.Validate(updatedGame, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gameFromDb = await _dbcontext.Game.FindAsync(id);

                if (gameFromDb == null)
                    return NoContent();

                gameFromDb.Title = updatedGame.Title;
                gameFromDb.PriceUsd = updatedGame.PriceUsd;
                gameFromDb.PublishDate = updatedGame.PublishDate;
                gameFromDb.AchievementsAmount = updatedGame.AchievementsAmount;
                gameFromDb.DeveloperID = updatedGame.DeveloperID;
                gameFromDb.Developer = updatedGame.Developer;

                await _dbcontext.SaveChangesAsync();

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
                GameLogoUpload gameLogoUpload = new GameLogoUpload(_configuration);

                var game = await _dbcontext.Game.FindAsync(id);

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

                await _dbcontext.SaveChangesAsync();

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
                var game = await _dbcontext.Game.FindAsync(id);

                if (game == null)
                    return NoContent();

                _dbcontext.Game.Remove(game);
                await _dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
