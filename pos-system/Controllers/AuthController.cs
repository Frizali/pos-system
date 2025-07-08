using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using System.Data;

namespace pos_system.Controllers
{
    public class AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<IActionResult> Index()
        {

            var userList = _userManager.Users.ToList();
            var Users = new List<UserModel>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "-",
                    IsActive = true
                });
            }

            return View(Users);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("User"))
                    return RedirectToAction("Index", "User");
                else
                    return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SweetAlert_Icon"] = "error";
                TempData["SweetAlert_Title"] = "Login Gagal";
                TempData["SweetAlert_Message"] = "Mohon lengkapi form dengan benar.";
                return View(model);
            }


            var user = await _userManager.FindByNameAsync(model.Name);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    TempData["SweetAlert_Icon"] = "success";
                    TempData["SweetAlert_Title"] = "Login Berhasil";
                    TempData["SweetAlert_Message"] = $"Selamat datang, {user.UserName}!";
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();

                    if (role != "User")
                        return RedirectToAction("Login", "Auth");
                    else
                        return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError("Password", "Invalid login attempt.");
            TempData["SweetAlert_Icon"] = "error";
            TempData["SweetAlert_Title"] = "Login Gagal";
            TempData["SweetAlert_Message"] = "Name atau Password salah.";
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["SweetAlert_Icon"] = "success";
                    TempData["SweetAlert_Title"] = "Register Berhasil";
                    TempData["SweetAlert_Message"] = $"Akun {user.UserName} berhasil dibuat.";

                    if (model.Role != "User")
                        return RedirectToAction("Login", "Auth");
                    else
                        return RedirectToAction("Index", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult NotFoundPage() => View();


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SweetAlert_Icon"] = "info";
            TempData["SweetAlert_Title"] = "Logout Berhasil";
            TempData["SweetAlert_Message"] = "Anda telah keluar dari sistem.";

            return RedirectToAction("Index", "User");
        }
    }
}
