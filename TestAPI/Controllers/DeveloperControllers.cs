using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
                    return Ok(JsonSerializer.Serialize(db.Developers));
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetDeveloperByID")]
        public IActionResult GetDeveloperByID(int id)
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
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteDeveloperByID/{id:int}")]
        public IActionResult DeleteDeveloperByID(int id)
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
                return NotFound(ex.Message);
            }
        }

        [HttpPost("PostNewDeveloper")]
        public IActionResult PostNewDeveloper(string name, string logoURL)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = new Developer { Name = name, LogoURL = logoURL, RegistrationDate = DateTime.Now, };
                    db.Developers.Add(dev);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("PutDeveloperName")]
        public IActionResult PutDeveloperName(int devID, string newName)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == devID).First();
                    dev.Name = newName;
                    db.SaveChanges();
                    return Ok();

                } 
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("PutDeveloperLogo")]
        public IActionResult PutDeveloperLogo(int devID, string newLogoURL)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == devID).First();
                    dev.Name = newLogoURL;
                    db.SaveChanges();
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
