using pos_system.Models;

namespace pos_system.Repository
{
    public class OrderRepo(ICrudRepo<TblOrder> repo) : IOrderRepo
    {
        readonly ICrudRepo<TblOrder> _repo = repo;

        public ICrudRepo<TblOrder> GetRepo()
        {
            return _repo;
        }
    }
}
