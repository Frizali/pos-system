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
            List<TblProductCategory> productCategories = await _categoryRepo.GetProductCategories().ConfigureAwait(false);
            return new ProductFormModelView
            {
                ProductCategories = productCategories
            };
        }
    }
}
