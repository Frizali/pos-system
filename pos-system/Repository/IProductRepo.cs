using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductRepo
    {
        Task<List<TblProduct>> GetList();
    }
}
