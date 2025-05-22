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

        public async Task<IActionResult> Save(ProductFormModel data, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ProductImage.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();

                        data.Product.ProductImage = Convert.ToBase64String(imageData);
                        data.Product.ImageType = Path.GetExtension(ProductImage.FileName)?.ToLower();
                    }
                }
                await _productService.Save(data);
            }
            return RedirectToAction("Index");
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

        public async Task<IActionResult> EditProduct(ProductFormModel data, IFormFile? ProductImage)
        {
            if (ProductImage != null && ProductImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ProductImage.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    data.Product.ProductImage = Convert.ToBase64String(imageData);
                    data.Product.ImageType = Path.GetExtension(ProductImage.FileName)?.TrimStart('.').ToLower();
                }
            }

            await _productService.EditProduct(data);

            return RedirectToAction("EditData", new { id = data.Product.ProductId });
        }

        public async Task<IActionResult> EditData(string id)
        {
            var product = await _productService.EditData(id).ConfigureAwait(false);
            return PartialView("Edit", product);
        }
    }
}
