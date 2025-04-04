using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
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

        public async Task<IActionResult> Save(ProductFormModelView data)
        {
            if (ModelState.IsValid)
            {
                await _productService.Save(data);
            }
            return RedirectToAction("Index");
        }
    }
}
