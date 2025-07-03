using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Services;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using pos_system.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace pos_system.Controllers
{
    [Authorize]
    public class OrderController(IOrderService orderService, IReportService reportService, IOrderNumberTrackerRepo orderNumberTrackerRepo, IOptions<MidtransSettings> midtrans, IConfiguration configuration) : Controller
    {
        readonly IOrderService _orderService = orderService;
        readonly IReportService _reportService = reportService;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;
        readonly MidtransSettings _midtrans = midtrans.Value;
        readonly IConfiguration _configuration= configuration;

        public async Task<IActionResult> CreateOrder(string orderId, bool? isTest)
        {
            _orderService.SetUsername(GetCurrentUserName());
            var orderDataJson = HttpContext.Session.GetString("OrderData");
            var orderNumber = HttpContext.Session.GetString("OrderNumber") ?? "ORD-000";
            var cashier = HttpContext.Session.GetString("Cashier") ?? "unknown";
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(orderDataJson))
                return BadRequest("Order data not found.");

            var snapParam = JsonConvert.DeserializeObject<SnapTokenModel>(orderDataJson);
            if (snapParam == null)
                return BadRequest("Invalid order data.");

            var order = new TblOrder
            {
                OrderId = orderId,
                OrderNumber = orderNumber,
                Status = "Success",
                Cashier = cashier,
                TotalPrice = snapParam.totalAmount,
                OrderDate = DateTime.Now,
                UserID = userId,
                Type = snapParam.type ?? "Cashier",
                ScheduledAt = snapParam.scheduledAt,
                Notes = snapParam.notes,
                PreOrderStatus = snapParam.type == "PreOrder" ? "Pending" : null,
                TblOrderItems = snapParam.TblOrderItems
            };

            await _orderService.CreateOrder(order);

            if(isTest!= null && isTest == true)
            {
                return StatusCode(200);
            }

            ReportModel base64PdfCustomer = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order" }).ConfigureAwait(false);
            ReportModel base64PdfKitchen = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order Kitchen" }).ConfigureAwait(false);

            var pdfBytesCustomer = Convert.FromBase64String(base64PdfCustomer.Data);
            var pdfBytesDapur = Convert.FromBase64String(base64PdfKitchen.Data);

            var mergedPdf = Unique.MergePdfs(pdfBytesCustomer, pdfBytesDapur);

            Response.Headers.Add("Content-Disposition", "inline; filename=Order.pdf");
            return File(mergedPdf, "application/pdf");
        }

        public async Task<IActionResult> UserCreateOrder(TblOrder order)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            order.UserID = userId;
            order.PreOrderStatus = order.Type == "PreOrder" ? "Pending" : null;
            _orderService.SetUsername(GetCurrentUserName());
            await _orderService.CreateOrder(order);
            ReportModel base64Pdf = await _reportService.GenerateReportPDF(new ReportParamModel() { ID = order.OrderId, FromDate = "", ToDate = "", ReportName = "Order" }).ConfigureAwait(false);
            string base64WithHeader = $"data:application/pdf;base64,{base64Pdf.Data}";
            return PartialView("_ShowPDF", base64WithHeader);
        }

        [HttpPost]
        public async Task<IActionResult> GetSnapToken([FromBody] SnapTokenModel param)
        {
            var orderNumber = await _orderNumberTrackerRepo.GetOrderNumber().ConfigureAwait(false);
            var cashier = GetCurrentUserName() ?? "";
            foreach (var item in param.TblOrderItems)
            {
                Console.WriteLine($"Item: {item.ProductId} Qty: {item.Quantity} Subtotal: {item.SubTotal}");
            }

            HttpContext.Session.SetString("OrderData", JsonConvert.SerializeObject(param));
            HttpContext.Session.SetString("OrderNumber", orderNumber);
            HttpContext.Session.SetString("Cashier", cashier);

            var payload = new
            {
                transaction_details = new
                {
                    order_id = param.orderId,
                    gross_amount = param.totalAmount
                },
                callbacks = new
                {
                    finish = Url.Action("CreateOrder", "Order", new { orderId = param.orderId }, Request.Scheme)
                }
            };

            using var client = new HttpClient();
            var serverKey = _midtrans.ServerKey;
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey + ":"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://app.sandbox.midtrans.com/snap/v1/transactions", content);
            var respString = await response.Content.ReadAsStringAsync();
            var respJson = JObject.Parse(respString);

            var snapToken = (string)respJson["token"];
            return Json(new { token = snapToken });
        }

        public IActionResult SnapFinish(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View("_Success");
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }

        public async Task<IActionResult> UpdatePreOrderStatus(string orderId, string status, string comment)
        {
            await _orderService.UpdatePreOrderStatus(orderId, status, comment).ConfigureAwait(false);
            return RedirectToAction("Index", "PreOrder");
        }
    }
}
