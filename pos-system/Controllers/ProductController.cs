using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        readonly IProductService _productService = productService;

        public async Task<IActionResult> Index()
        {
            var viewModel = await _productService.ProductFormModel().ConfigureAwait(false);
            return View(viewModel);
        }

        public async Task<IActionResult> Save(ProductFormModel data, IFormFile productImage)
        {
            var viewModel = await _productService.ProductFormModel().ConfigureAwait(false);
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.Save(data, productImage);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var modelError = ex.Message.Split(",");
                    ModelState.AddModelError("Product." + modelError[0], modelError[1]);
                    data.ProductCategories = viewModel.ProductCategories;
                    return View("Index", data);
                }
            }
            else
            {
                data.ProductCategories = viewModel.ProductCategories;
                return View("Index", data);
            }
        }

        public async Task<IActionResult> ProductList(string? category, string? product)
        {
            var viewModel = await _productService.ProductListViewModel(category, product).ConfigureAwait(false);
            return View(viewModel);
        }

        public async Task<IActionResult> ProductDetailByID(string id)
        {
            var data = await _productService.ProductDetailByID(id).ConfigureAwait(false);
            return PartialView("_ProductDetail", data);
        }

        public async Task<IActionResult> EditProduct(ProductFormModel data, IFormFile? productImage)
        {
            await _productService.EditProduct(data, productImage);
            return RedirectToAction("EditData", new { id = data.Product.ProductId });
        }

        public async Task<IActionResult> EditData(string id)
        {
            var product = await _productService.EditData(id).ConfigureAwait(false);
            return View("Edit", product);
        }
    }
}
