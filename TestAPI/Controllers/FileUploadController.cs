using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;

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
                DeveloperProfilePictureUpload developerProfilePictureUpload = new DeveloperProfilePictureUpload();

                if (developer == null)
                    return NoContent();

                if (logo == null)
                {
                    developer.LogoURL = $"{developerProfilePictureUpload.BucketUrl}{developerProfilePictureUpload.Placeholder}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo,  guid).Wait();
                    bucket.DeleteObject(developer.LogoURL!).Wait();

                    developer.LogoURL = $"{developerProfilePictureUpload.BucketUrl}{guid}";
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
                GameLogoUpload gameLogoUpload = new GameLogoUpload();

                if (game == null)
                    return NoContent();

                if (logo == null)
                {
                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{gameLogoUpload.Placeholder}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, guid).Wait();
                    bucket.DeleteObject(game.LogoURL!).Wait();

                    game.LogoURL = $"{gameLogoUpload.BucketUrl}{guid}";
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
                UserProfilePictureUpload userProfilePictureUpload = new UserProfilePictureUpload();
                
                if (user == null)
                    return NoContent();

                if (logo == null)
                {
                    user.ProfilePictureURL = $"{userProfilePictureUpload.BucketUrl}{userProfilePictureUpload.Placeholder}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, guid).Wait();
                    bucket.DeleteObject(user.ProfilePictureURL!).Wait();

                    user.ProfilePictureURL = $"{userProfilePictureUpload.BucketUrl}{guid}";
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
