using pos_system.Models;

namespace pos_system.Services
{
    public interface IProductService
    {
        Task<ProductFormModel> ProductFormModel();
        Task<ProductListViewModel> ProductListViewModel(string? category, string? product);
        Task Save(ProductFormModel data, IFormFile? productImage);
        Task<TblProduct> ProductDetailByID(string id);
        Task EditProduct(ProductFormModel data, IFormFile? productImage);
        Task<ProductFormModel> EditProductModal(string id); 
    }
}
