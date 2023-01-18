using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class GameControllers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
