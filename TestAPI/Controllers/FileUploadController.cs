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

        public FileUploadController(ApplicationDbContext context)
        {
            dbcontext = context;
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
                    developer.LogoURL = $"{S3Bucket.DeveloperBucketUrl}{S3Bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.DeveloperBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(developer.LogoURL!, S3Bucket.DeveloperBucketPath).Wait();

                    developer.LogoURL = $"{S3Bucket.DeveloperBucketUrl}{guid}";
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
                    game.LogoURL = $"{S3Bucket.GameBucketUrl}{S3Bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.GameBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(game.LogoURL!, S3Bucket.GameBucketPath).Wait();

                    game.LogoURL = $"{S3Bucket.GameBucketUrl}{guid}";
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
                    user.ProfilePictureURL = $"{S3Bucket.UserBucketUrl}{S3Bucket.DefaultLogoName}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.UserBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(user.ProfilePictureURL!, S3Bucket.UserBucketPath).Wait();

                    user.ProfilePictureURL = $"{S3Bucket.GameBucketUrl}{guid}";
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
