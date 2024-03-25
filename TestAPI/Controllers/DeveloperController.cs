using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using WebAPI.Services.S3Bucket.Developer;
using WebAPI.Services.Validation.DeveloperValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/Developer")]
    public class DeveloperController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IDeveloperValidation _developerValidation;
        private readonly IS3Bucket _bucket;
        private readonly IConfiguration _configuration;

        public DeveloperController(ApplicationDbContext dbcontext, IS3Bucket bucket, IDeveloperValidation developerValidation, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _developerValidation = developerValidation;
            _bucket = bucket;
            _configuration = configuration;
        }

        [HttpGet("GetAllDevelopers")]
        public async Task<ActionResult<IEnumerable<Developer>>> GetAllDevelopers()
        {
            try
            {
                return Ok(await _dbcontext.Developer.ToListAsync());
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
                var developer = await _dbcontext.Developer.FindAsync(id);

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
                _developerValidation.Validate(newDeveloper, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                DeveloperProfilePictureUpload developerLogoUpload = new DeveloperProfilePictureUpload(_configuration);
                newDeveloper.LogoURL = $"{developerLogoUpload.BucketUrl}{developerLogoUpload.Placeholder}";

                await _dbcontext.Developer.AddAsync(newDeveloper);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostDeveloper), new { id = newDeveloper.ID }, newDeveloper);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloper/{id:int}")]
        public async Task<ActionResult<Developer>> PutDeveloper(int id, [FromBody] Developer updatedDeveloper)
        {
            try
            {
                updatedDeveloper.ID = id;
                _developerValidation.Validate(updatedDeveloper, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var developerFromDb = await _dbcontext.Developer.FindAsync(id);

                if (developerFromDb == null)
                    return NoContent();

                developerFromDb.Name = updatedDeveloper.Name;
                developerFromDb.CreationDate = updatedDeveloper.CreationDate;

                await _dbcontext.SaveChangesAsync();

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
                DeveloperProfilePictureUpload developerLogoUpload = new DeveloperProfilePictureUpload(_configuration);

                var developer = await _dbcontext.Developer.FindAsync(id);

                if (developer == null)
                    return NoContent();

                if (developer.LogoURL != $"{developerLogoUpload.BucketUrl}{developerLogoUpload.Placeholder}")
                    await developerLogoUpload.DeleteObject(developer.LogoURL!);

                if (logo == null)
                {
                    developer.LogoURL = $"{developerLogoUpload.BucketUrl}{developerLogoUpload.Placeholder}";
                }
                else
                {
                    Guid newLogoGuid = Guid.NewGuid();

                    await developerLogoUpload.AddObject(logo, newLogoGuid);

                    developer.LogoURL = $"{developerLogoUpload.BucketUrl}{newLogoGuid}";
                }

                await _dbcontext.SaveChangesAsync();

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
                var developer = await _dbcontext.Developer.FindAsync(id);

                if (developer == null)
                    return NoContent();

                _dbcontext.Developer.Remove(developer);
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
