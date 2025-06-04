using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    [Authorize]
    public class ActivityController(IOrderService orderService, ILogAuditService logService) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogAuditService _logService = logService;
        public async Task<IActionResult> Order(string? fromDate, string? toDate)
        {
            List<TblOrder> history = await _orderService.GetOrderHistory(fromDate, toDate).ConfigureAwait(false);
            return View(history);
        }

        public async Task<IActionResult> LogAudit(string? logAction, string? entity, string? key, string? fromDate, string? toDate)
        {
            List<TblLogAudit> logs = await _logService.GetLogs(logAction, entity, key, fromDate, toDate).ConfigureAwait(false);
            return View(logs);
        }
    }
}
