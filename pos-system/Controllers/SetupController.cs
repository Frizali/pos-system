using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class SetupController(ISetupService setupService) : Controller
    {
        private readonly ISetupService _setupService = setupService;

        public async Task<IActionResult> Index()
        {
            var setup = await _setupService.GetSetup().ConfigureAwait(false);
            return View(setup);
        }

        public async Task<IActionResult> Save(TblSetup data)
        {
            _setupService.SetUsername(GetCurrentUserName());
            await _setupService.UpdateSetup(data).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }
    }
}
