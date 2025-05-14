using pos_system.Models;

namespace pos_system.Repository
{
    public interface IOrderRepo
    {
        ICrudRepo<TblOrder> GetRepo();
    }
}
