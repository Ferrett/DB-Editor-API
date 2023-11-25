using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.S3Bucket.Developer;
using WebAPI.Services.Validation.DeveloperValidation;
using WebAPI.Services.Validation.UserValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Developer")]
    public class DeveloperController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly IDeveloperValidation developerValidation;
        private readonly IS3Bucket bucket;
        private readonly IConfiguration configuration;

        public DeveloperController(ApplicationDbContext context, IS3Bucket _bucket, IDeveloperValidation _developerValidation, IConfiguration _configuration)
        {
            dbcontext = context;
            developerValidation = _developerValidation;
            bucket = _bucket;
            configuration = _configuration;
        }

        [HttpGet("GetDevelopers")]
        public async Task<ActionResult<IEnumerable<Developer>>> GetDevelopers()
        {
            try
            {
                return Ok(await dbcontext.Developer.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDeveloper/{id:int}")]
        public async Task<ActionResult<Developer>> GetDeveloper(int id)
        {
            try
            {
                var developer = await dbcontext.Developer.FindAsync(id);

                if (developer == null)
                    return NoContent();

                return Ok(developer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostDeveloper")]
        public async Task<ActionResult<Developer>> PostDeveloper([FromBody] Developer newDeveloper)
        {
            try
            {
                developerValidation.Validate(newDeveloper, dbcontext.Developer.ToList(), ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.Developer.AddAsync(newDeveloper);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostDeveloper), new { id = newDeveloper.ID }, newDeveloper);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloper/{id:int}")]
        public async Task<ActionResult<Developer>> PutDeveloper(int id, [FromBody] Developer newDeveloper)
        {
            try
            {
                developerValidation.Validate(newDeveloper, dbcontext.Developer.ToList(), ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var developerFromDb = await dbcontext.Developer.FindAsync(id);

                if (developerFromDb == null)
                    return NoContent();

                developerFromDb.Name = newDeveloper.Name;
                developerFromDb.LogoURL = newDeveloper.LogoURL;
                developerFromDb.RegistrationDate = newDeveloper.RegistrationDate;
                developerFromDb.PublishedGames = newDeveloper.PublishedGames;

                await dbcontext.SaveChangesAsync();

                return Ok(developerFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloperLogo/{id:int}")]
        public async Task<ActionResult<Developer>> PutDeveloperLogo(int id, IFormFile? logo = null)
        {
            try
            {
                DeveloperProfilePictureUpload developerProfilePictureUpload = new DeveloperProfilePictureUpload(configuration);

                var developer = await dbcontext.Developer.FindAsync(id);

                if (developer == null)
                    return NoContent();

                if (logo == null)
                {
                    developer.LogoURL = $"{developerProfilePictureUpload.BucketUrl}{developerProfilePictureUpload.Placeholder}";
                }
                else
                {
                    Guid guid = Guid.NewGuid();

                    bucket.AddObject(logo, guid).Wait();
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


        [HttpDelete("DeleteDeveloper/{id:int}")]
        public async Task<ActionResult<Developer>> DeleteDeveloper(int id)
        {
            try
            {
                var developer = await dbcontext.Developer.FindAsync(id);

                if (developer == null)
                    return NoContent();

                dbcontext.Developer.Remove(developer);
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
