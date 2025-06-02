using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

public class ManageAccController : Controller
{
    public IActionResult ManageAcc()
    {
        var model = new ManageAccFormModel
        {
            Email = "kasir1@example.com",
            Name = "Kasir1",
        };

        return View("~/Views/User/ManageAcc.cshtml", model);
    }
}
