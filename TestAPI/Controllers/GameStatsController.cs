using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.Validation.GameStatsValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
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

        [HttpGet("GetGamesStats")]
        public async Task<ActionResult<IEnumerable<GameStats>>> GetGamesStats()
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
                gameStatsValidation.Validate(newGameStats, dbcontext.GameStats.ToList(), ModelState);

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
        public async Task<ActionResult<GameStats>> PutGameStats(int id, [FromBody] GameStats newGameStats)
        {
            try
            {
                gameStatsValidation.Validate(newGameStats, dbcontext.GameStats.ToList(), ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gameStatsFromDb = await dbcontext.GameStats.FindAsync(id);

                if (gameStatsFromDb == null)
                    return NoContent();

                gameStatsFromDb.UserID = newGameStats.UserID;
                gameStatsFromDb.User = newGameStats.User;
                gameStatsFromDb.GameID = newGameStats.GameID;
                gameStatsFromDb.Game = newGameStats.Game;
                gameStatsFromDb.HoursPlayed = newGameStats.HoursPlayed;
                gameStatsFromDb.AchievementsGot = newGameStats.AchievementsGot;
                gameStatsFromDb.PurchaseDate = newGameStats.PurchaseDate;

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
