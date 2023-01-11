using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                User tom = new User { Name = "Tom", Age = 33 };
                User alice = new User { Name = "Alice", Age = 26 };

                // добавляем их в бд
                db.Users.Add(tom);
                db.Users.Add(alice);
                db.SaveChanges();
            }
            return "ye";
        }
    }
}

