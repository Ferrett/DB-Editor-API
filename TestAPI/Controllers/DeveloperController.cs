using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Developer")]
    public class DeveloperController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public DeveloperController(ApplicationDbContext context)
        {
            dbcontext = context;
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

        [HttpGet("GetDeveloper{id:int}")]
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
        public async Task<ActionResult<Developer>> PostDeveloper([FromBody] Developer developer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                dbcontext.Developer.Add(developer);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(PostDeveloper), new { id = developer.ID }, developer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloper/{id:int}")]
        public async Task<ActionResult<Developer>> PutDeveloper(int id, [FromBody] Developer developer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != developer.ID)
                    return BadRequest();

                var developerFromDb = await dbcontext.Developer.FindAsync(id);

                if (developerFromDb == null)
                    return NoContent();

                developerFromDb.Name = developer.Name;
                developerFromDb.LogoURL = developer.LogoURL;
                developerFromDb.RegistrationDate = developer.RegistrationDate;
                developerFromDb.PublishedGames = developer.PublishedGames;

                await dbcontext.SaveChangesAsync();

                return Ok(developerFromDb);
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
                var match = await dbcontext.Developer.FindAsync(id);

                if (match == null)
                    return NoContent();

                dbcontext.Developer.Remove(match);
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
