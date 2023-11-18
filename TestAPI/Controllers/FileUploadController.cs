using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/FileUpload")]
    public class FileUploadController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IS3Bucket bucket;

        public FileUploadController(ApplicationDbContext _context, IS3Bucket _bucket)
        {
            dbcontext = _context;
            bucket = _bucket;
        }

        [HttpPost("UpdateDeveloperLogo")]
        public async Task<ActionResult<FileUploadController>> UpdateDeveloperLogo(int developerID, IFormFile? logo = null)
        {
            try
            {
                var developer = await dbcontext.Developer.FindAsync(developerID);

                if (developer == null)
                    return NoContent();

                if (logo == null)
                {
                    developer.LogoURL = $"{bucket.DeveloperBucketUrl}{bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, bucket.DeveloperBucketPath, guid).Wait();
                    bucket.DeleteObject(developer.LogoURL!, bucket.DeveloperBucketPath).Wait();

                    developer.LogoURL = $"{bucket.DeveloperBucketUrl}{guid}";
                }

                await dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateGameLogo")]
        public async Task<ActionResult<FileUploadController>> UpdateGameLogo(int gameID, IFormFile? logo = null)
        {
            try
            {
                var game = await dbcontext.Game.FindAsync(gameID);

                if (game == null)
                    return NoContent();

                if (logo == null)
                {
                    game.LogoURL = $"{bucket.GameBucketUrl}{bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, bucket.GameBucketPath, guid).Wait();
                    bucket.DeleteObject(game.LogoURL!, bucket.GameBucketPath).Wait();

                    game.LogoURL = $"{bucket.GameBucketUrl}{guid}";
                }

                await dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateUserProfilePicture")]
        public async Task<ActionResult<FileUploadController>> UpdateUserProfilePicture(int userID, IFormFile? logo = null)
        {
            try
            {
                var user = await dbcontext.User.FindAsync(userID);

                if (user == null)
                    return NoContent();

                if (logo == null)
                {
                    user.ProfilePictureURL = $"{bucket.UserBucketUrl}{bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, bucket.UserBucketPath, guid).Wait();
                    bucket.DeleteObject(user.ProfilePictureURL!, bucket.UserBucketPath).Wait();

                    user.ProfilePictureURL = $"{bucket.GameBucketUrl}{guid}";
                }

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
