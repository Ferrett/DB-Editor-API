using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
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

        public GameController(ApplicationDbContext context, IGameValidation _gameValidation)
        {
            dbcontext = context;
            gameValidation = _gameValidation;
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
                gameValidation.Validate(newGame, dbcontext.Game, ModelState);

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
                gameValidation.Validate(newGame, dbcontext.Game, ModelState);

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
