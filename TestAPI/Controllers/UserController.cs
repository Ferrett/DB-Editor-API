using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.S3Bucket.User;
using WebAPI.Services.Validation.UserValidation;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IUserValidation _userValidation;
        private readonly IS3Bucket _bucket;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext dbcontext, IS3Bucket bucket, IUserValidation userValidation, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _userValidation = userValidation;
            _bucket = bucket;
            _configuration = configuration;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                return Ok(await _dbcontext.User.ToListAsync());
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
                var user = await _dbcontext.User.FindAsync(id);

                if (user == null)
                    return NoContent();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetUserByLogin/{userLogin}")]
        public async Task<ActionResult<User>> GetUserByLogin(string userLogin)
        {
            try
            {
                var user = await _dbcontext.User.FirstOrDefaultAsync(x=>x.Login == userLogin);

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
                _userValidation.Validate(newUser, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                UserProfilePictureUpload userPfpUpload = new UserProfilePictureUpload(_configuration);
                newUser.ProfilePictureURL = $"{userPfpUpload.BucketUrl}{userPfpUpload.Placeholder}";

                await _dbcontext.User.AddAsync(newUser);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostUser), new { id = newUser.ID }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUser/{id:int}")]
        public async Task<ActionResult<User>> PutUser(int id, [FromBody] User upadtedUser)
        {
            try
            {
                upadtedUser.ID = id;
                _userValidation.Validate(upadtedUser, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userFromDb = await _dbcontext.User.FindAsync(id);

                if (userFromDb == null)
                    return NoContent();

                userFromDb.Login = upadtedUser.Login;
                userFromDb.Password = upadtedUser.Password;
                userFromDb.Nickname = upadtedUser.Nickname;
                userFromDb.Email = upadtedUser.Email;
                userFromDb.CreationDate = upadtedUser.CreationDate;

                await _dbcontext.SaveChangesAsync();

                return Ok(userFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutUserProfilePicture/{id:int}")]
        public async Task<ActionResult<User>> PutUserProfilePicture(int id, IFormFile? logo = null)
        {
            try
            {
                UserProfilePictureUpload userPfpUpload = new UserProfilePictureUpload(_configuration);

                var user = await _dbcontext.User.FindAsync(id);

                if (user == null)
                    return NoContent();

                if (user.ProfilePictureURL != $"{userPfpUpload.BucketUrl}{userPfpUpload.Placeholder}")
                    await userPfpUpload.DeleteObject(user.ProfilePictureURL!);

                if (logo == null)
                {
                    user.ProfilePictureURL = $"{userPfpUpload.BucketUrl}{userPfpUpload.Placeholder}";
                }
                else
                {
                    Guid newPfpGuid = Guid.NewGuid();

                    await userPfpUpload.AddObject(logo, newPfpGuid);

                    user.ProfilePictureURL = $"{userPfpUpload.BucketUrl}{newPfpGuid}";
                }

                await _dbcontext.SaveChangesAsync();

                return Ok(user);
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
                var user = await _dbcontext.User.FindAsync(id);

                if (user == null)
                    return NoContent();

                _dbcontext.User.Remove(user);
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

