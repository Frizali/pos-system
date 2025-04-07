using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductRepo
    {
        ICrudRepo<TblProduct> GetRepo();
        Task<List<TblProduct>> GetAllProductDetails();
    }
}
