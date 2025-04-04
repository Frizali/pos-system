using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class ProductService : IProductService
    {
        readonly IProductCategoryRepo _categoryRepo;
        readonly IProductRepo _productRepo;
        readonly IVariantGroupRepo _variantGroupRepo;
        private int codeLength = 4;
        public ProductService(IProductCategoryRepo categoryRepo, IProductRepo productRepo, IVariantGroupRepo variantGroupRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _variantGroupRepo = variantGroupRepo;
        }

        public async Task<ProductFormModelView> GetProductFormModelView()
        {
            List<TblProductCategory> productCategories = await _categoryRepo.GetRepo().GetAll().ConfigureAwait(false);
            return new ProductFormModelView
            {
                ProductCategories = productCategories
            };
        }

        public async Task Save(ProductFormModelView data)
        {
            var productId = Guid.NewGuid().ToString();
            data.Product.ProductId = productId;

            SetProductCode(data);
            SetVariantGroups(data);
            SetProductVariants(data);

            await _productRepo.GetRepo().Add(data.Product).ConfigureAwait(false);
            await SetVariantOptions(data).ConfigureAwait(false);
        }

        private void SetVariantGroups(ProductFormModelView data)
        {
            if (data.VariantGroups.Count() > 0)
            {
                foreach (var group in data.VariantGroups)
                {
                    group.GroupId = Guid.NewGuid().ToString();
                    group.ProductId = data.Product.ProductId;
                }
            }
        }

        private async Task SetVariantOptions(ProductFormModelView data)
        {
            if(data.ProductVariants.Count() > 0)
            {
                var variantOptionSplit = data.ProductVariants.Select(x => x.Sku.Split("-")).ToList();
                var totalVariant = variantOptionSplit.Select(x => x).FirstOrDefault().Select(x => x).Count();

                for (var i = 0; i < totalVariant; i++)
                {
                    var variantOption = variantOptionSplit.Select(x => x[i]).Distinct().ToList();
                    var variantGroup = data.VariantGroups[i];
                    var variantGroupId = variantGroup.GroupId;

                    foreach (var option in variantOption)
                    {
                        TblVariantOption tblVariantOption = new TblVariantOption
                        {
                            OptionId = Guid.NewGuid().ToString(),
                            GroupId = variantGroupId,
                            Value = option
                        };

                        variantGroup.TblVariantOptions.Add(tblVariantOption);
                    }
                }

                await _variantGroupRepo.GetRepo().AddRange(data.VariantGroups).ConfigureAwait(false);
            }
        }

        private void SetProductVariants(ProductFormModelView data)
        {
            if(data.ProductVariants.Count() > 0)
            {
                foreach (var variant in data.ProductVariants)
                {
                    variant.VariantId = Guid.NewGuid().ToString();
                    variant.ProductId = data.Product.ProductId;
                };

                data.Product.TblProductVariants = data.ProductVariants;
            }
        }

        private void SetProductCode(ProductFormModelView data)
        {
            var clearedProductName = data.Product.ProductName.Replace(" ", "");
            var productCodeLength = clearedProductName.Length;

            if (productCodeLength > codeLength)
            {
                var codeDevide = productCodeLength / codeLength;
                var rawCode = String.Empty;

                for (var i = 0; i < productCodeLength - codeDevide; i += codeDevide)
                {
                    rawCode += clearedProductName[i];
                }

                data.Product.ProductCode = rawCode.ToUpper();
            }else if (productCodeLength == 4)
            {
                data.Product.ProductCode = data.Product.ProductName.ToUpper();
            }
            else
            {
                data.Product.ProductCode = "N/A";
            }
        }
    }
}
