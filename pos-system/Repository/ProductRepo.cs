using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductRepo : IProductRepo
    {
        readonly AppDbContext _context;
        readonly IMapper _mapper;
        readonly ICrudRepo<TblProduct> _repo;
        public ProductRepo(AppDbContext context, IMapper mapper, ICrudRepo<TblProduct> repo)
        {
            _context = context;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<List<ProductDTO>> GetAllProductDetailsDTO()
        {
            var products = await _context.TblProduct.Where(p => p.IsAvailable).ProjectTo<ProductDTO>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);    
            var productCategories = await _context.TblProductCategory.ProjectTo<ProductCategoryDTO>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);
            var productVariants = await _context.TblProductVariant.ProjectTo<ProductVariantDTO>(_mapper.ConfigurationProvider).GroupBy(pv => pv.ProductId).ToListAsync().ConfigureAwait(false);
            var result = products.Join(productCategories, p => p.CategoryId, pc => pc.CategoryId, (p,pc) => { p.Category = pc; return p; }).ToList();
            result = result.Join(productVariants, p => p.ProductId, pv => pv.Key, (p, pv) => { p.ProductVariants = pv.ToList(); return p; }).ToList();
            return products;
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
