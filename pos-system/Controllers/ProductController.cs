using Microsoft.AspNetCore.Mvc;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class ProductController : Controller
    {
        readonly IProductService _productService;
        public ProductController(IProductService productServices)
        {
            _productService = productServices;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = await _productService.GetProductFormModelView().ConfigureAwait(false);
            return View(viewModel);
        }
    }
}
