using Microsoft.AspNetCore.Mvc;
using WebAPI.Logic;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserControllers : Controller
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                Validation.ValidateList(new ApplicationContext().User);

                return Ok(new ApplicationContext().User.ToList());
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
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    return Ok(user);
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
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    db.User.Remove(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Post/{login}/{password}/{nickname}")]
        public IActionResult Post(string login, string password, string nickname, string? email = null, IFormFile? logo = null)
        {
            try
            {
                Validation.ValidateLogin(login);
                Validation.ValidatePassword(password);
                Validation.ValidateNameLength(nickname);
                Validation.ValidateEmail(email);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = new User
                    {
                        Login = login,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        Nickname = nickname,
                        Email = email == null ? null : email,
                        CreationDate = DateTime.UtcNow,
                    };

                    if (logo == null)
                    {
                        user.AvatarURL = $"{S3Bucket.UserBucketUrl}{S3Bucket.DefaultLogoName}";
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();
                        S3Bucket.AddObject(logo, S3Bucket.UserBucketPath, guid).Wait();
                        user.AvatarURL = $"{S3Bucket.UserBucketUrl}{guid}";
                    }

                    db.User.Add(user);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutLogin/{id:int}/{login}")]
        public IActionResult PutLogin(int id, string login)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateLogin(login);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    user.Login = login;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutPassword/{id:int}/{password}")]
        public IActionResult PutPassword(int id, string password)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidatePassword(password);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutNickname/{id:int}/{nickname}")]
        public IActionResult PutNickname(int id, string nickname)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateNameLength(nickname);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    user.Nickname = nickname;

                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutEmail/{id:int}/{email}")]
        public IActionResult PutEmail(int id, string email)
        {
            try
            {
                Validation.ValidateUserID(id);
                Validation.ValidateEmail(email);

                using (ApplicationContext db = new ApplicationContext())
                {
                    User user = db.User.Where(x => x.ID == id).First();
                    user.Email = email;

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
                Validation.ValidateUserID(id);

                using (ApplicationContext db = new ApplicationContext())
                {
                    Guid guid = Guid.NewGuid();

                    S3Bucket.AddObject(logo, S3Bucket.UserBucketPath, guid).Wait();
                    S3Bucket.DeleteObject(db.User.Where(x => x.ID == id).First().AvatarURL, S3Bucket.UserBucketPath).Wait();

                    User user = db.User.Where(x => x.ID == id).First();
                    user.AvatarURL = $"{S3Bucket.UserBucketUrl}{guid}";
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

