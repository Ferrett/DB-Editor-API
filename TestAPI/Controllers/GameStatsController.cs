using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.GameStatsValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/GameStats")]
    public class GameStatsController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IGameStatsValidation gameStatsValidation;
        public GameStatsController(ApplicationDbContext context, IGameStatsValidation _gameStatsValidation)
        {
            dbcontext = context;
            gameStatsValidation = _gameStatsValidation;
        }

        [HttpGet("GetAllGamesStats")]
        public async Task<ActionResult<IEnumerable<GameStats>>> GetAllGamesStats()
        {
            try
            {
                return Ok(await dbcontext.GameStats.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGameStats/{id:int}")]
        public async Task<ActionResult<GameStats>> GetGameStats(int id)
        {
            try
            {
                var gameStats = await dbcontext.GameStats.FindAsync(id);

                if (gameStats == null)
                    return NoContent();

                return Ok(gameStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostGameStats")]
        public async Task<ActionResult<GameStats>> PostGameStats([FromBody] GameStats newGameStats)
        {
            try
            {
                gameStatsValidation.Validate(newGameStats, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.GameStats.AddAsync(newGameStats);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostGameStats), new { id = newGameStats.ID }, newGameStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameStats/{id:int}")]
        public async Task<ActionResult<GameStats>> PutGameStats(int id, [FromBody] GameStats updatedGameStats)
        {
            try
            {
                updatedGameStats.ID = id;
                gameStatsValidation.Validate(updatedGameStats, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gameStatsFromDb = await dbcontext.GameStats.FindAsync(id);

                if (gameStatsFromDb == null)
                    return NoContent();

                gameStatsFromDb.UserID = updatedGameStats.UserID;
                gameStatsFromDb.User = updatedGameStats.User;
                gameStatsFromDb.GameID = updatedGameStats.GameID;
                gameStatsFromDb.Game = updatedGameStats.Game;
                gameStatsFromDb.HoursPlayed = updatedGameStats.HoursPlayed;
                gameStatsFromDb.AchievementsGotten = updatedGameStats.AchievementsGotten;
                gameStatsFromDb.PurchaseDate = updatedGameStats.PurchaseDate;

                await dbcontext.SaveChangesAsync();

                return Ok(gameStatsFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteGameStats/{id:int}")]
        public async Task<ActionResult<GameStats>> DeleteGameStats(int id)
        {
            try
            {
                var gameStats = await dbcontext.GameStats.FindAsync(id);

                if (gameStats == null)
                    return NoContent();

                dbcontext.GameStats.Remove(gameStats);
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
