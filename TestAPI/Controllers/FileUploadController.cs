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

        [HttpPost("PostDeveloperLogo")]
        public async Task<ActionResult<FileUploadController>> PostDeveloperLogo(int developerID, IFormFile? logo = null)
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

                    Developer dev = dbcontext.Developer.Where(x => x.ID == developerID).First();
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

    }
}
