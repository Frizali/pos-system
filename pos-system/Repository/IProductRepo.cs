using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductRepo
    {
        ICrudRepo<TblProduct> GetRepo();
        Task<List<ProductDTO>> GetAllProductDetailsDTO();
        Task<TblProduct> GetMenuDetailById(string id);
    }
}
