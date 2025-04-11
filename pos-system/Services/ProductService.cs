using pos_system.Helpers;
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

        public async Task<List<TblProduct>> GetAllProductDetails()
        {
            return await _productRepo.GetAllProductDetails().ConfigureAwait(false);
        }   

        public async Task<ProductFormModel> GetProductFormModelView()
        {
            List<TblProductCategory> productCategories = await _categoryRepo.GetRepo().GetAll().ConfigureAwait(false);
            return new ProductFormModel
            {
                ProductCategories = productCategories
            };
        }

        public async Task<MenuViewModel> GetMenuViewModel()
        {
            var products = await _productRepo.GetAllProductDetails().ConfigureAwait(false);
            var productCategories = await _categoryRepo.GetRepo().GetAll().ConfigureAwait(false);

            productCategories.Select(x => { x.TblProducts = products.Where(p => p.CategoryId == x.CategoryId).ToList(); return x; });

            return new MenuViewModel
            {
                products = products,
                productCategories = productCategories
            };
        }

        public async Task Save(ProductFormModel data)
        {
            data.Product.ProductId = Unique.ID();
            data.Product.ProductCode = Unique.GenerateCode(data.Product.ProductName,codeLength);

            SetVariantGroups(data);
            SetProductVariants(data);
            SetProductAddons(data);

            await _productRepo.GetRepo().Add(data.Product).ConfigureAwait(false);
            await SetVariantOptions(data).ConfigureAwait(false);
        }

        private void SetVariantGroups(ProductFormModel data)
        {
            if (data.VariantGroups.Count() > 0)
                data.VariantGroups = data.VariantGroups.Select(x => { x.GroupId = Unique.ID(); x.ProductId = data.Product.ProductId; return x; }).ToList();
        }

        private void SetProductVariants(ProductFormModel data)
        {
            if (data.ProductVariants.Count() > 0)
                data.Product.TblProductVariants = data.ProductVariants.Select(x => { x.VariantId = Unique.ID(); x.ProductId = data.Product.ProductId; return x; }).ToList();
        }

        private void SetProductAddons(ProductFormModel data)
        {
            if(data.ProductAddons.Count() > 0)
                data.Product.TblProductAddons = data.ProductAddons.Select(x => { x.AddonId = Unique.ID(); x.ProductId = data.Product.ProductId; return x; }).ToList();
        }

        private async Task SetVariantOptions(ProductFormModel data)
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

        public async Task<TblProduct> GetMenuDetailById(string id)
        {
            return await _productRepo.GetMenuDetailById(id).ConfigureAwait(false);
        }
    }
}
