using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductCategoryRepo
    {
        Task<List<TblProductCategory>> GetProductCategories();
    }
}
