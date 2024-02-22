using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.Sockets;
using WebAPI.Logic;
using WebAPI.Migrations;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.S3Bucket.User;
using WebAPI.Services.Validation.UserGameValidation;
using WebAPI.Services.Validation.UserValidation;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/UserGame")]
    public class UserGameController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IUserGameValidation _userGameValidation;

        public UserGameController(ApplicationDbContext dbcontext, IUserGameValidation userGameValidation)
        {
            _dbcontext = dbcontext;
            _userGameValidation = userGameValidation;
        }

        [HttpGet("GetAllUserGames")]
        public async Task<ActionResult<IEnumerable<UserGame>>> GetAllUserGames()
        {
            try
            {
                return Ok(await _dbcontext.UserGame.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGamesByUserID/{userID:int}")]
        public async Task<ActionResult<UserGame>> GetGamesByUserID(int userID)
        {
            try
            {
                var games = await _dbcontext.UserGame.Where(x => x.UserID == userID).Select(x => x.Game).ToListAsync();

                if (games == null)
                    return NoContent();

                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersByGameID/{gameID:int}")]
        public async Task<ActionResult<UserGame>> GetUsersByGameID(int gameID)
        {
            try
            {
                var games = await _dbcontext.UserGame.Where(x => x.GameID == gameID).Select(x => x.Game).ToListAsync();

                if (games == null)
                    return NoContent();

                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUserGame")]
        public async Task<ActionResult<UserGame>> PostUserGame([FromBody] UserGame newUserGame)
        {
            try
            {
                _userGameValidation.Validate(newUserGame, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _dbcontext.UserGame.AddAsync(newUserGame);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostUserGame), null, newUserGame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUserGame/{id:int}")]
        public async Task<ActionResult<UserGame>> DeleteUserGame(int id)
        {
            try
            {
                var userGame = await _dbcontext.UserGame.FindAsync(id);

                if (userGame == null)
                    return NoContent();

                _dbcontext.UserGame.Remove(userGame);
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
