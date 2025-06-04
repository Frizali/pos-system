using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    [Authorize]
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
            _productService.SetUsername(GetCurrentUserName());
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

        public ActionResult ToggleModifyMode()
        {
            if(HttpContext.Session.GetString("ModifyMode") is null)
            {
                HttpContext.Session.SetString("ModifyMode", "false");
            }
            var mode = HttpContext.Session.GetString("ModifyMode");
            if (mode == "true")
            {
                HttpContext.Session.SetString("ModifyMode", "false");
            }
            else
            {
                HttpContext.Session.SetString("ModifyMode", "true");
            }
            return RedirectToAction("ProductList");
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
            _productService.SetUsername(GetCurrentUserName());
            await _productService.EditProduct(data, productImage);
            return RedirectToAction("EditData", new { id = data.Product.ProductId });
        }

        public async Task<IActionResult> EditData(string id)
        {
            var product = await _productService.EditData(id).ConfigureAwait(false);
            return View("Edit", product);
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }
    }
}
