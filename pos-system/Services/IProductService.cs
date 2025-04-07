using pos_system.Models;

namespace pos_system.Services
{
    public interface IProductService
    {
        Task<ProductFormModelView> GetProductFormModelView();
        Task Save(ProductFormModelView data);
        Task<List<TblProduct>> GetAllProductDetails();
    }
}
