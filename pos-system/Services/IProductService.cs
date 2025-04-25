using pos_system.Models;

namespace pos_system.Services
{
    public interface IProductService
    {
        Task<ProductFormModel> GetProductFormModelView();
        Task<MenuViewModel> GetMenuViewModel(string? category, string? product);
        Task Save(ProductFormModel data);
        Task<List<TblProduct>> GetAllProductDetails();
        Task<TblProduct> GetMenuDetailById(string id);
    }
}
