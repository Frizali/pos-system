using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Helpers;
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
            _orderService.SetUsername(GetCurrentUserName());
            await _orderService.CreateOrder(order);
            ReportModel base64PdfCustomer = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order" }).ConfigureAwait(false);
            ReportModel base64PdfKitchen = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order Kitchen" }).ConfigureAwait(false);

            //byte[] pdfBytes = Convert.FromBase64String(base64Pdf.Data);

            //Response.Headers.Add("Content-Disposition", "inline; filename=Order.pdf");
            //return File(pdfBytes, "application/pdf");

            var pdfBytesCustomer = Convert.FromBase64String(base64PdfCustomer.Data);
            var pdfBytesDapur = Convert.FromBase64String(base64PdfKitchen.Data);

            var mergedPdf = Unique.MergePdfs(pdfBytesCustomer, pdfBytesDapur);

            Response.Headers.Add("Content-Disposition", "inline; filename=Order.pdf");
            return File(mergedPdf, "application/pdf");
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }
    }
}
