using AutoMapper;
using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;
using System.Data.Common;

namespace pos_system.Services
{
    public class ProductService(IProductCategoryRepo categoryRepo, IProductRepo productRepo, IVariantGroupRepo variantGroupRepo) : IProductService
    {
        readonly IProductCategoryRepo _categoryRepo = categoryRepo;
        readonly IProductRepo _productRepo = productRepo;
        readonly IVariantGroupRepo _variantGroupRepo = variantGroupRepo;
        readonly int codeLength = 4;

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
                ProductCategories = productCategoriesDTO
            };
        }

        public async Task Save(ProductFormModel data)
        {
            data.Product.ProductCode = Unique.GenerateCode(data.Product.ProductName,codeLength);
            SetVariant(data);

            await _productRepo.GetRepo().Add(data.Product).ConfigureAwait(false);
            await SetVariantOptions(data).ConfigureAwait(false);
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
                var totalVariant = variantOptionSplit.Select(x => x).Count();

                for (var i = 0; i < totalVariant; i++)
                {
                    var variantOption = variantOptionSplit.Select(x => x[i]).Distinct().ToList();
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
    }
}
