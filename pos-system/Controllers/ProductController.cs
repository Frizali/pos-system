﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_system.Models;
using pos_system.Services;

namespace pos_system.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        readonly IProductService _productService = productService;

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _productService.ProductFormModel().ConfigureAwait(false);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Save(ProductFormModel data, IFormFile productImage)
        {
            _productService.SetUsername(GetCurrentUserName());
            var viewModel = await _productService.ProductFormModel().ConfigureAwait(false);

            if (productImage != null && productImage.Length > 400 * 1024)
            {
                ModelState.AddModelError("Product.ProductImage", "The image size must be no more than 400KB.");
                data.ProductCategories = viewModel.ProductCategories;
                return View("Index", data);
            }
            if (productImage == null || productImage.Length == 0)
            {
                ModelState.AddModelError("Product.ProductImage", "Product image is required.");
                data.ProductCategories = viewModel.ProductCategories;
                return View("Index", data);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.Save(data, productImage);
                    TempData["SuccessMessage"] = "Product added successfully.";
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

        [Authorize(Roles = "Admin,Manager")]
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
            return RedirectToAction("ProductList", new { category= "All" });
        }

        [Authorize]
        public async Task<IActionResult> ProductList(string? category = "All", string? product = "")
        {
            var viewModel = await _productService.ProductListViewModel(category, product).ConfigureAwait(false);
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetailByID(string id)
        {
            var data = await _productService.ProductDetailByID(id).ConfigureAwait(false);
            return PartialView("_ProductDetail", data);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditProduct(ProductFormModel data, IFormFile? productImage)
        {
            _productService.SetUsername(GetCurrentUserName());
            await _productService.EditProduct(data, productImage);
            return RedirectToAction("EditData", new { id = data.Product.ProductId });
        }

        [Authorize(Roles = "Admin,Manager")]
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
