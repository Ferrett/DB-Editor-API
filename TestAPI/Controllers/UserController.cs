using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public UserController(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return Ok(await dbcontext.User.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetUser/{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await dbcontext.User.FindAsync(id);

                if (user == null)
                    return NoContent();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUser")]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.User.AddAsync(user);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostUser), new { id = user.ID }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUser/{id:int}")]
        public async Task<ActionResult<User>> PutUser(int id, [FromBody] User User)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != User.ID)
                    return BadRequest();

                var userFromDb = await dbcontext.User.FindAsync(id);

                if (userFromDb == null)
                    return NoContent();

                userFromDb.Login = User.Login;
                userFromDb.PasswordHash = User.PasswordHash;
                userFromDb.Nickname = User.Nickname;
                userFromDb.ProfilePictureURL = User.ProfilePictureURL;
                userFromDb.Email = User.Email;
                userFromDb.CreationDate = User.CreationDate;
                userFromDb.GamesStats = User.GamesStats;

                await dbcontext.SaveChangesAsync();

                return Ok(userFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser/{id:int}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {
                var user = await dbcontext.User.FindAsync(id);

                if (user == null)
                    return NoContent();

                dbcontext.User.Remove(user);
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

