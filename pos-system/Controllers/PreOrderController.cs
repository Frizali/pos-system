using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class PreOrderController(IOrderService orderService, UserManager<ApplicationUser> userManager) : Controller
    {
        readonly IOrderService _orderService = orderService;
        readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<IActionResult> Index(string? fromDate, string? toDate, string? status = "All")
        {
            var user = await _userManager.FindByNameAsync(GetCurrentUserName());
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var preOrders = await _orderService.GetPreOrder(fromDate, toDate, status, userId, role).ConfigureAwait(false);

            return View(preOrders);
        }

        public async Task<IActionResult> History(string? fromDate, string? toDate, string status = "All")
        {
            var user = await _userManager.FindByNameAsync(GetCurrentUserName());
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var preOrders = await _orderService.GetPreOrder(fromDate, toDate, status, userId, role).ConfigureAwait(false);

            return View(preOrders);
        }

        public async Task<IActionResult> UserOrder(string? fromDate, string? toDate, string status = "All")
        {
            var user = await _userManager.FindByNameAsync(GetCurrentUserName());
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var preOrders = await _orderService.GetPreOrder(fromDate, toDate, status, userId, role).ConfigureAwait(false);

            return View("~/Views/User/Order.cshtml", preOrders);
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }
    }
}
