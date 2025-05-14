using pos_system.Models;

namespace pos_system.Services
{
    public interface IProductService
    {
        Task<ProductFormModel> GetProductFormModelView();
        Task<MenuViewModel> GetMenuViewModel(string? category, string? product);
        Task Save(ProductFormModel data);
        Task<TblProduct> GetMenuDetailById(string id);
    }
}
