﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class ActivityController(IOrderService orderService, ILogAuditService logService, IInventoryService inventoryService) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogAuditService _logService = logService;
        private readonly IInventoryService _inventoryService = inventoryService;

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Order(string? fromDate, string? toDate)
        {
            List<TblOrder> history = await _orderService.GetOrderHistory(fromDate, toDate).ConfigureAwait(false);
            ActivityViewModel historyViewModel = new ActivityViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                TblOrders = history
            };
            return View(historyViewModel);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> LogAudit(string? logAction, string? entity, string? key, string? fromDate, string? toDate)
        {
            List<TblLogAudit> logs = await _logService.GetLogs(logAction, entity, key, fromDate, toDate).ConfigureAwait(false);
            ActivityViewModel historyViewModel = new ActivityViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                TblLogAudits = logs
            };
            return View(historyViewModel);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PartMovement(string partId, string partTypeId, DateTime date, string month, string year)
        {
            var logs = await _inventoryService.GetListPartMovement(partId, partTypeId, date, month, year).ConfigureAwait(false);
            return View(logs);
        }
    }
}
