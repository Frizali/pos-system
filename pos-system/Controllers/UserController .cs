using Microsoft.AspNetCore.Mvc;

namespace pos_system.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
