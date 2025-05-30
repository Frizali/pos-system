using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    [Authorize]
    public class OrderController(IOrderService orderService, IReportService reportService) : Controller
    {
        readonly IOrderService _orderService = orderService;
        readonly IReportService _reportService = reportService;
        public async Task<IActionResult> CreateOrder(TblOrder order)
        {
            await _orderService.CreateOrder(order);
            ReportModel base64Pdf = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order" }).ConfigureAwait(false);
            string base64WithHeader = $"data:application/pdf;base64,{base64Pdf.Data}";

            return PartialView("_ShowPDF", base64WithHeader);
        }
    }
}
