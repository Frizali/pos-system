using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    
    public class DashboardController(IDashboardService dashboardService, IReportService reportService) : Controller
    {
        readonly IDashboardService _dashboardService = dashboardService;
        readonly IReportService _reportService = reportService;

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index(string? fromDate, string? toDate)
        {
            DashboardViewModel dashboardViewModel = await _dashboardService.DashboardViewModel(fromDate, toDate);
            return View(dashboardViewModel);
        }

        [Authorize(Roles = "Admin,Manager,User")]
        [HttpPost]
        public async Task<IActionResult> DownloadReport([FromBody]ReportParamModel param)
        {
            ReportModel base64Pdf = await _reportService.GenerateReportPDF(param).ConfigureAwait(false);
            byte[] bytes = Convert.FromBase64String(base64Pdf.Data);
            return File(bytes, "application/pdf", "Sales.pdf");
        }
    }
}
