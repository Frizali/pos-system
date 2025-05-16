using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductCategoryRepo
    {
        ICrudRepo<TblProductCategory> GetRepo();
        Task<List<ProductCategoryDTO>> ProductCategoriesDTO();
    }
}
