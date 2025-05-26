using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductVariantRepo(AppDbContext context, ICrudRepo<TblProductVariant> repo) : IProductVariantRepo
    {
        readonly ICrudRepo<TblProductVariant> _repo = repo;
        readonly AppDbContext _context = context;

        public ICrudRepo<TblProductVariant> GetRepo()
        {
            return _repo;
        }
    }
}
