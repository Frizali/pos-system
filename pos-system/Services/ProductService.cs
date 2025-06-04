using Microsoft.EntityFrameworkCore;
using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class ProductService(IProductCategoryRepo categoryRepo, IProductRepo productRepo, IVariantGroupRepo variantGroupRepo, IOrderNumberTrackerRepo orderNumberTrackerRepo) : IProductService
    {
        readonly IProductCategoryRepo _categoryRepo = categoryRepo;
        readonly IProductRepo _productRepo = productRepo;
        readonly IVariantGroupRepo _variantGroupRepo = variantGroupRepo;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;
        readonly int codeLength = 4;
        private string _username = "System";

        public void SetUsername(string username)
        {
            _username = username;
            _categoryRepo.GetRepo().SetUsername(username);
            _productRepo.GetRepo().SetUsername(username);
            _variantGroupRepo.GetRepo().SetUsername(username);
        }

        public async Task<ProductFormModel> ProductFormModel()
        {
            List<TblProductCategory> productCategories = await _categoryRepo.GetRepo().GetAll().ConfigureAwait(false);
            return new ProductFormModel
            {
                ProductCategories = productCategories
            };
        }

        public async Task<ProductListViewModel> ProductListViewModel(string? category, string? product)
        {
            var products = await _productRepo.ProductDetailsDTO().ConfigureAwait(false);
            var productCategoriesDTO = await _categoryRepo.ProductCategoriesDTO().ConfigureAwait(false);
            var orderNumber = await _orderNumberTrackerRepo.GetOrderNumber().ConfigureAwait(false);
            ProductCategoryDTO allProductCategory = new ()
            {
                CategoryId = "All",
                CategoryName = "All",
                CategoryDescription = "All",
                TotalProducts = productCategoriesDTO.Sum(pc => pc.TotalProducts)
            };

            productCategoriesDTO.Add(allProductCategory);

            if (!String.IsNullOrEmpty(category) && category != "All")
                products = products.Where(p => p.CategoryId == category).ToList();

            if (!String.IsNullOrEmpty(product))
                products = products.Where(p => p.ProductName.Contains(product, StringComparison.OrdinalIgnoreCase)).ToList();

            return new ProductListViewModel
            {
                Products = products,
                ProductCategories = productCategoriesDTO,
                OrderNumber = orderNumber
            };
        }

        public async Task Save(ProductFormModel data, IFormFile? productImage)
        {
            data.Product.ProductCode = Unique.GenerateCode(data.Product.ProductName,codeLength);
            SetVariant(data);

            await SetImage(data, productImage).ConfigureAwait(false);
            await _productRepo.GetRepo().Add(data.Product).ConfigureAwait(false);
            await SetVariantOptions(data).ConfigureAwait(false);
        }

        private static async Task SetImage(ProductFormModel data, IFormFile? productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                int maxSize = 400 * 1024;

                if (productImage.Length > maxSize)
                    throw new Exception("ProductImage, Image size exceeds the maximum limit of 400KB.");

                using var memoryStream = new MemoryStream();
                await productImage.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                data.Product.ProductImage = Convert.ToBase64String(imageData);
                data.Product.ImageType = Path.GetExtension(productImage.FileName)?.ToLower();
            }
        }

        private static void SetVariant(ProductFormModel data)
        {
            if (data.VariantGroups is not null && data.VariantGroups.Count > 0)
                data.VariantGroups = data.VariantGroups.Select(x => { x.ProductId = data.Product.ProductId; return x; }).ToList();

            if (data.ProductVariants is not null && data.ProductVariants.Count > 0)
                data.Product.TblProductVariants = data.ProductVariants.Select(x => { x.ProductId = data.Product.ProductId; x.Sku = x.Sku.Trim(); return x; }).ToList();

            if (data.ProductAddons is not null && data.ProductAddons.Count > 0)
                data.Product.TblProductAddons = data.ProductAddons.Select(x => { x.ProductId = data.Product.ProductId; return x; }).ToList();
        }

        private async Task SetVariantOptions(ProductFormModel data)
        {
            if(data.ProductVariants is not null && data.ProductVariants.Count > 0 && data.VariantGroups is not null && data.VariantGroups.Count > 0)
            {
                var variantOptionSplit = data.ProductVariants.Select(x => x.Sku.Split("-")).ToList();
                var totalVariant = variantOptionSplit.First().Count();

                for (var i = 0; i < totalVariant; i++)
                {
                    var variantOption = variantOptionSplit.Select(x => x[i].Trim()).Distinct().ToList();
                    var variantGroup = data.VariantGroups[i];
                    var variantGroupId = variantGroup.GroupId;

                    foreach (var option in variantOption)
                    {
                        TblVariantOption tblVariantOption = new()
                        {
                            OptionId = Unique.ID(),
                            GroupId = variantGroupId,
                            Value = option
                        };

                        variantGroup.TblVariantOptions.Add(tblVariantOption);
                    }
                }

                await _variantGroupRepo.GetRepo().AddRange(data.VariantGroups).ConfigureAwait(false);
            }
        }

        public async Task<TblProduct> ProductDetailByID(string id)
        {
            return await _productRepo.ProductDetailByID(id).ConfigureAwait(false);
        }

        public async Task EditProduct(ProductFormModel data, IFormFile? productImage)
        {
            await SetImage(data, productImage).ConfigureAwait(false);
            await _productRepo.EditProduct(data, _username).ConfigureAwait(false);
        }

        public async Task<ProductFormModel> EditData(string id)
        {
            return await _productRepo.EditData(id).ConfigureAwait(false);
        }
    }
}
