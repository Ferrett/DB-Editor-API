using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ReviewControllers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
