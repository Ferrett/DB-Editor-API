using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.Validation.UserValidation;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IUserValidation userValidation;

        public UserController(ApplicationDbContext context, IUserValidation _userValidation)
        {
            dbcontext = context;
            userValidation = _userValidation;
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
        public async Task<ActionResult<User>> PostUser([FromBody] User newUser)
        {
            try
            {
                userValidation.Validate(newUser,dbcontext.User,ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.User.AddAsync(newUser);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostUser), new { id = newUser.ID }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUser/{id:int}")]
        public async Task<ActionResult<User>> PutUser(int id, [FromBody] User newUser)
        {
            try
            {
                userValidation.Validate(newUser, dbcontext.User, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userFromDb = await dbcontext.User.FindAsync(id);

                if (userFromDb == null)
                    return NoContent();

                userFromDb.Login = newUser.Login;
                userFromDb.PasswordHash = newUser.PasswordHash;
                userFromDb.Nickname = newUser.Nickname;
                userFromDb.ProfilePictureURL = newUser.ProfilePictureURL;
                userFromDb.Email = newUser.Email;
                userFromDb.CreationDate = newUser.CreationDate;
                userFromDb.GamesStats = newUser.GamesStats;

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

