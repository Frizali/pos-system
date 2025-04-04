using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductRepo : IProductRepo
    {
        readonly AppDbContext _context;
        readonly ICrudRepo<TblProduct> _repo;
        public ProductRepo(AppDbContext context, ICrudRepo<TblProduct> repo)
        {
           _context = context;
           _repo = repo;
        }

        public ICrudRepo<TblProduct> GetRepo()
        {
            return _repo;
        }
    }
}
