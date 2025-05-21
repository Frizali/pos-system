using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

namespace pos_system.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            
            var users = new List<UserViewModel>
            {
                new UserViewModel { Id = 1, Email = "admin@example.com", Role = "Manager" },
                new UserViewModel { Id = 2, Email = "kasir1@example.com", Role = "Kasir" }
            };

            return View(users);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
