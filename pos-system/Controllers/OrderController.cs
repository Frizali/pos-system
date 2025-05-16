using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class OrderController(IOrderService orderService) : Controller
    {
        readonly IOrderService _orderService = orderService;
        public async Task<IActionResult> CreateOrder(TblOrder order)
        {
            await _orderService.CreateOrder(order);
            return RedirectToAction("ProductList", "Product");
        }
    }
}
