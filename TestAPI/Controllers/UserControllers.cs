//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllers : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Getamogus(int id)
        {
            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    // создаем два объекта User
            //    User tom = new User { Name = "Tom", Age = 33 };
            //    User alice = new User { Name = "Alice", Age = 26 };

            //    // добавляем их в бд
            //    db.Users.Add(tom);
            //    db.Users.Add(alice);
            //    db.SaveChanges();
            //}
            return NotFound();
        }

        [HttpGet("doblaeb/{id:int}")]
        public IActionResult GetAllUsers(int id)
        {
            //{
            //    var listEmployees = new List<Employee>()
            //            {
            //                new Employee(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
            //                new Employee(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
            //                new Employee(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR"},
            //            };
            //    if (listEmployees.Count > 0)
            //    {
            //        return Ok(listEmployees);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }

            return Ok(22);
        }

    }
}

