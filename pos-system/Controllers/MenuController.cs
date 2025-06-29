using Microsoft.AspNetCore.Mvc;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class MenuController(IProductService productService) : Controller
    {
        readonly IProductService _productService = productService;
            
        public async Task<IActionResult> Index(string? category = "All", string? product = "")
        {
            var viewModel = await _productService.ProductListViewModel(category, product).ConfigureAwait(false);
            return View(viewModel);
        }
    }
}
