using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductCategoryRepo : IProductCategoryRepo
    {
        readonly AppDbContext _context;
        readonly ICrudRepo<TblProductCategory> _repo;
        public ProductCategoryRepo(AppDbContext context, ICrudRepo<TblProductCategory> repo)
        {
            _context = context;
            _repo = repo;
        }

        public ICrudRepo<TblProductCategory> GetRepo()
        {
            return _repo;
        }   
    }
}
