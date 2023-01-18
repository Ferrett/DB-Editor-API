//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class UserControllers : Controller
    {
        [HttpGet("/")]
        public IActionResult Getamogus()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
               

                // добавляем их в бд
                db.Developers.Add(
                    new Developer { Name = "fist",RegistrationDate = DateTime.Now, LogoURL= @"https://i.ytimg.com/vi/ZINZLJpU9pw/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLCVoegkrckRIuMJOhlemHUG10eWUg" });
                
                db.SaveChanges();
            }
            return NotFound();
        }

        [HttpGet("test/{id:int}")]
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

