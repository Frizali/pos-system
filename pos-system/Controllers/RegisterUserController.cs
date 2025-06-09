using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

public class RegisterUserController : Controller
{
    [HttpGet]
    public IActionResult RegisterUser()
    {
        return View("~/Views/Auth/RegisterUser.cshtml");
    }

    [HttpPost]
    public IActionResult Index(RegisterUserModel model)
    {
        // Dummy response 
        ViewBag.Message = "Form submitted!";
        ViewBag.MessageType = "success";

        return View("~/Views/Auth/RegisterUser.cshtml", model);
    }
}