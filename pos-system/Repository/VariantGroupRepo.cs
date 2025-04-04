using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class VariantGroupRepo : IVariantGroupRepo
    {
        readonly AppDbContext _context;
        readonly ICrudRepo<TblVariantGroup> _repo;
        public VariantGroupRepo(AppDbContext context, ICrudRepo<TblVariantGroup> repo)
        {
            _context = context;
            _repo = repo;
        }

        public ICrudRepo<TblVariantGroup> GetRepo()
        {
            return _repo;
        }
    }
}
