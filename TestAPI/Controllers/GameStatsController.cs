using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/GameStats")]
    public class GameStatsController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public GameStatsController(ApplicationDbContext context)
        {
            dbcontext = context;
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
        public async Task<ActionResult<GameStats>> PostGameStats([FromBody] GameStats gameStats)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.GameStats.AddAsync(gameStats);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostGameStats), new { id = gameStats.ID }, gameStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGameStats/{id:int}")]
        public async Task<ActionResult<GameStats>> PutGameStats(int id, [FromBody] GameStats gameStats)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != gameStats.ID)
                    return BadRequest();

                var gameStatsFromDb = await dbcontext.GameStats.FindAsync(id);

                if (gameStatsFromDb == null)
                    return NoContent();

                gameStatsFromDb.UserID = gameStats.UserID;
                gameStatsFromDb.User = gameStats.User;
                gameStatsFromDb.GameID = gameStats.GameID;
                gameStatsFromDb.Game = gameStats.Game;
                gameStatsFromDb.HoursPlayed = gameStats.HoursPlayed;
                gameStatsFromDb.AchievementsGot = gameStats.AchievementsGot;
                gameStatsFromDb.PurchaseDate = gameStats.PurchaseDate;

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
