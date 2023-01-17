using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpPost(Name = "GWF")]
        public string Getamogus()
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

