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

        public async Task<List<TblProduct>> GetAllProductDetails()
        {
            //return await _context.TblProduct
            //    .Include(p => p.Category)
            //    .Include(p => p.TblVariantGroups)
            //        .ThenInclude(v => v.TblVariantOptions)
            //    .Include(p => p.TblProductVariants)
            //    .ToListAsync().ConfigureAwait(false);

            var products = await (from p in _context.TblProduct
                                  join c in _context.TblProductCategory on p.CategoryId equals c.CategoryId
                                  select new { p, c }).ToListAsync().ConfigureAwait(false);

            var variantGroups = await (from vg in _context.TblVariantGroup select vg).ToListAsync().ConfigureAwait(false);
            var variantOptions = await (from vo in _context.TblVariantOption select vo).ToListAsync().ConfigureAwait(false);
            var productVariants = await (from pv in _context.TblProductVariant select pv).ToListAsync().ConfigureAwait(false);

            var finalVariantGroups = variantGroups.Select(vg => { vg.TblVariantOptions = variantOptions.Where(vo => vo.GroupId == vg.GroupId).ToList(); return vg; }).ToList();

            var finalProducts = products.Select(x =>
                {
                    x.p.Category = x.c;
                    x.p.TblVariantGroups = finalVariantGroups.Where(vg => vg.ProductId == x.p.ProductId).ToList();
                    x.p.TblProductVariants = productVariants.Where(pv => pv.ProductId == x.p.ProductId).ToList();

                    return x.p;
                }
            ).ToList();

            return finalProducts;
        }

        public async Task<TblProduct> GetMenuDetailById(string id)
        {
            return await _context.TblProduct
                .Where(p => p.ProductId == id)
                .Include(p => p.Category)
                .Include(p => p.TblVariantGroups)
                    .ThenInclude(v => v.TblVariantOptions)
                .Include(p => p.TblProductVariants)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public ICrudRepo<TblProduct> GetRepo()
        {
            return _repo;
        }
    }
}
