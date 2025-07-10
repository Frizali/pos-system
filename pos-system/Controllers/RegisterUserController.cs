using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

public class RegisterUserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet]
    public IActionResult RegisterUser()
    {
        return View("~/Views/Auth/RegisterUser.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Index(RegisterFormModel model)
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

                return RedirectToAction("Login", "Auth");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("~/Views/Auth/RegisterUser.cshtml", model);
    }
}