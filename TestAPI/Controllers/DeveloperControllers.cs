using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Developer")]
    public class DeveloperControllers : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().Developers);

                return Ok(new ApplicationContext().Developers.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                Validation.ValidateDeveloperID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    return Ok(dev);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Validation.ValidateDeveloperID(id);

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

        [HttpPost("Post/{name}")]
        public IActionResult Post(string name)
        {
            try
            {
                Validation.ValidateDeveloperName(name);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Developer dev = new Developer
                    {
                        Name = name,
                        LogoURL = $"{S3Bucket.DeveloperBucketUrl}{S3Bucket.DefaultLogoName}",
                        RegistrationDate = DateTime.UtcNow,
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

        [HttpPut("PutName/{id:int}/{name}")]
        public IActionResult PutName(int id, string name)
        {
            try
            {
                Validation.ValidateDeveloperName(name);
                Validation.ValidateDeveloperID(id);

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

        [HttpPut("PutLogo/{id:int}")]
        public IActionResult PutLogo(int id, IFormFile logo)
        {
            try
            {
                Validation.ValidateDeveloperID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.DeveloperBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(db.Developers.Where(x => x.ID == id).First().LogoURL, S3Bucket.DeveloperBucketPath).Wait();

                    Developer dev = db.Developers.Where(x => x.ID == id).First();
                    dev.LogoURL = $"{S3Bucket.DeveloperBucketUrl}{guid}";
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
