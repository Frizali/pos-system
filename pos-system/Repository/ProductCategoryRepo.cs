using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductCategoryRepo : IProductCategoryRepo
    {
        readonly AppDbContext _context;
        public ProductCategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TblProductCategory>> GetProductCategories()
        {
            return await _context.TblProductCategory.ToListAsync().ConfigureAwait(false);
        }
    }
}
