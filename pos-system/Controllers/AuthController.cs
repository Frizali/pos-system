using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using SixLabors.Fonts;
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
                if (roles.FirstOrDefault() == "User") continue;
                Users.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "-",
                    IsActive = user.IsActive
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
                TempData["SweetAlert_Title"] = "Login failed.";
                TempData["SweetAlert_Message"] = "Please complete the form correctly.";
                return View(model);
            }


            var user = await _userManager.FindByNameAsync(model.Name);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    TempData["SweetAlert_Icon"] = "error";
                    TempData["SweetAlert_Title"] = "Login failed.";
                    TempData["SweetAlert_Message"] = "Your account has been deactivated.";
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    TempData["SweetAlert_Icon"] = "success";
                    TempData["SweetAlert_Title"] = "Login successful.";
                    TempData["SweetAlert_Message"] = $"Welcome, {user.UserName}!";
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
            TempData["SweetAlert_Title"] = "Login failed.";
            TempData["SweetAlert_Message"] = "Incorrect username or password.";
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
                    TempData["SweetAlert_Title"] = "Registration successful.";
                    TempData["SweetAlert_Message"] = $"{user.UserName} account has been successfully created.";

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

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> InActiveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
                TempData["StatusMessage"] = $"User {user.UserName} has been deactivated.";
                TempData["StatusType"] = "warning";
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ActiveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                TempData["StatusMessage"] = $"User {user.UserName} has been activated.";
                TempData["StatusType"] = "success";
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult NotFoundPage() => View();


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SweetAlert_Icon"] = "info";
            TempData["SweetAlert_Title"] = "Logout successful.";
            TempData["SweetAlert_Message"] = "You have been logged out of the system.";

            return RedirectToAction("Index", "User");
        }
    }
}
