using Microsoft.AspNetCore.Mvc;

namespace pos_system.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
