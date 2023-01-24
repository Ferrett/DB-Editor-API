using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
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
                Validation.ValidateList(new ApplicationContext().Developers);

                using (ApplicationContext db = new ApplicationContext())
                {
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
                Validation.ValidateDeveloperID(id);

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

        [HttpPost("PostDeveloper/{name}")]
        public IActionResult PostDeveloper(string name)
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

        [HttpPut("PutDeveloperName/{id:int}/{name}")]
        public IActionResult PutDeveloperName(int id, string name)
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

        [HttpPut("PutDeveloperLogo/{id:int}")]
        public IActionResult PutDeveloperLogo(int id, IFormFile logo)
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
