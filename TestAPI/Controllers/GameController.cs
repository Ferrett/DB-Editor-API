using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Game")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public GameController(ApplicationDbContext context)
        {
            dbcontext = context;
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
        public async Task<ActionResult<Game>> PostGame([FromBody] Game game)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.Game.AddAsync(game);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostGame), new { id = game.ID }, game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutGame/{id:int}")]
        public async Task<ActionResult<Game>> PutGame(int id, [FromBody] Game game)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != game.ID)
                    return BadRequest();

                var gameFromDb = await dbcontext.Game.FindAsync(id);

                if (gameFromDb == null)
                    return NoContent();

                gameFromDb.Name = game.Name;
                gameFromDb.LogoURL = game.LogoURL;
                gameFromDb.Price = game.Price;
                gameFromDb.PublishDate= game.PublishDate;
                gameFromDb.AchievementsCount= game.AchievementsCount;
                gameFromDb.DeveloperID= game.DeveloperID;
                gameFromDb.Developer= game.Developer;
                gameFromDb.Reviews= game.Reviews;

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
