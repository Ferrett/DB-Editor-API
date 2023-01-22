using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    public class DeveloperControllers : Controller
    {
        [HttpGet("GetAllDevelopers")]
        public IActionResult GetAllDevelopers()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.Developers.ToArray().Length == 0)
                        throw new ArgumentNullException("Sequence has no elements");

                    return Ok(db.Developers.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDeveloper/{id:int}")]
        public IActionResult GetDeveloper(int id)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    return Ok(dev);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteDeveloper/{id:int}")]
        public IActionResult DeleteDeveloper(int id)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    db.Developers.Remove(dev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostDeveloper/{name}")]
        public IActionResult PostDeveloper(string name, IFormFile logo)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                S3Publish.WritingAnObjectAsync(logo,@"webapilogos/developer",guid).Wait();
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = new Developer { 
                        Name = name, 
                        LogoURL = @$"https://webapilogos.s3.eu-north-1.amazonaws.com/{guid}", 
                        RegistrationDate = DateTime.Now, 
                    };

                    db.Developers.Add(dev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloperName/{id:int}/{name}")]
        public IActionResult PutDeveloperName(int id, string name)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    dev.Name = name;
                    db.SaveChanges();
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutDeveloperLogo/{id:int}/{logo}")]
        public IActionResult PutDeveloperLogo(int id, IFormFile logo)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    dev.Name = logo.FileName;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
