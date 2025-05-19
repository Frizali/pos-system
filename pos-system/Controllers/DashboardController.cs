using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        readonly IDashboardService _dashboardService = dashboardService;

        public async Task<IActionResult> Index(string? fromDate, string? toDate)
        {
            DashboardViewModel dashboardViewModel = await _dashboardService.DashboardViewModel(fromDate, toDate);
            return View(dashboardViewModel);
        }
    }
}
