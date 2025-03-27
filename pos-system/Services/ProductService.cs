using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class ProductService : IProductService
    {
        readonly IProductCategoryRepo _categoryRepo;
        public ProductService(IProductCategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public async Task<ProductFormModelView> GetProductFormModelView()
        {
            List<TblProductCategory> productCategories = await _categoryRepo.GetList().ConfigureAwait(false);
            return new ProductFormModelView
            {
                ProductCategories = productCategories
            };
        }

        public async Task Save(ProductFormModelView data)
        {
            var productId = Guid.NewGuid().ToString();
            data.Product.ProductId = productId;

            foreach(var variant in data.ProductVariants)
            {
                variant.VariantId = Guid.NewGuid().ToString();
                variant.ProductId = productId;
            };

            
        }
    }
}
