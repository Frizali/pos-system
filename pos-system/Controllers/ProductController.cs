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

        public async Task<IActionResult> Save(ProductFormModel data)
        {
            if (ModelState.IsValid)
            {
                await _productService.Save(data);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Menu(string? category, string? product)
        {
            var viewModel = await _productService.GetMenuViewModel(category, product).ConfigureAwait(false);
            return View(viewModel);
        }

        public async Task<IActionResult> GetDetailMenu(string id)
        {
            var data = await _productService.GetMenuDetailById(id).ConfigureAwait(false);
            return PartialView("_ProductDetail", data);
        }
    }
}
