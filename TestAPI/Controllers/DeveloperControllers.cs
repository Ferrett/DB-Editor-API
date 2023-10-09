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
                Validation.ValidateList(new ApplicationDbContext().Developer);

                return Ok(new ApplicationDbContext().Developer.ToList());
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

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Developer dev = db.Developer.Where(x => x.ID == id).First();
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

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Developer dev = db.Developer.Where(x => x.ID == id).First();
                    db.Developer.Remove(dev);
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
        public IActionResult Post(string name, IFormFile? logo=null)
        {
            try
            {
                Validation.ValidateDeveloperName(name);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Developer dev = new Developer
                    {
                        Name = name,       
                        RegistrationDate = DateTime.UtcNow,
                    };

                    if(logo==null)
                    {
                        dev.LogoURL = $"{S3Bucket.DeveloperBucketUrl}{S3Bucket.DefaultLogoName}";
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();
                        S3Bucket.AddObject(logo, S3Bucket.DeveloperBucketPath, guid).Wait();
                        dev.LogoURL = $"{S3Bucket.DeveloperBucketUrl}{guid}";
                    }

                    db.Developer.Add(dev);
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

                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    Developer dev = db.Developer.Where(x => x.ID == id).First();
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

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.DeveloperBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(db.Developer.Where(x => x.ID == id).First().LogoURL, S3Bucket.DeveloperBucketPath).Wait();

                    Developer dev = db.Developer.Where(x => x.ID == id).First();
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
