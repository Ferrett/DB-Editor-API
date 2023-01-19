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
                    if (db.Developers.ToArray().Length == 0)
                        throw new ArgumentNullException("Sequence has no elements");

                    return Ok(JsonSerializer.Serialize(db.Developers));
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetDeveloperByID/{id:int}")]
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

        [HttpPost("PostNewDeveloper/{name}/{logo}")]
        public IActionResult PostNewDeveloper(string name, string logo)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = new Developer { Name = name, LogoURL = logo, RegistrationDate = DateTime.Now, };
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
                return NotFound(ex.Message);
            }
        }

        [HttpPut("PutDeveloperLogo/{id:int}/{logo}")]
        public IActionResult PutDeveloperLogo(int id, string logo)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    dev.Name = logo;
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
