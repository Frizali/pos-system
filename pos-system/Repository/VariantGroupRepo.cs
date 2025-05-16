using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class VariantGroupRepo(ICrudRepo<TblVariantGroup> repo) : IVariantGroupRepo
    {
        readonly ICrudRepo<TblVariantGroup> _repo = repo;

        public ICrudRepo<TblVariantGroup> GetRepo()
        {
            return _repo;
        }
    }
}
