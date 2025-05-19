using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductRepo(AppDbContext context, IMapper mapper, ICrudRepo<TblProduct> repo) : IProductRepo
    {
        readonly AppDbContext _context = context;
        readonly IMapper _mapper = mapper;
        readonly ICrudRepo<TblProduct> _repo = repo;

        public async Task<List<ProductDTO>> ProductDetailsDTO()
        {
            var products = await _context.TblProduct.Where(p => p.IsAvailable).ProjectTo<ProductDTO>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);    
            var productCategories = await _context.TblProductCategory.ProjectTo<ProductCategoryDTO>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);
            var productVariants = await _context.TblProductVariant.ProjectTo<ProductVariantDTO>(_mapper.ConfigurationProvider).GroupBy(pv => pv.ProductId).ToListAsync().ConfigureAwait(false);
            var result = products.Join(productCategories, p => p.CategoryId, pc => pc.CategoryId, (p,pc) => { p.Category = pc; return p; }).ToList();
            result = result.Join(productVariants, p => p.ProductId, pv => pv.Key, (p, pv) => { p.ProductVariants = [.. pv]; return p; }).ToList();
            return products;
        }

        public async Task<TblProduct> ProductDetailByID(string id)
        {
            return await _context.TblProduct
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id).ConfigureAwait(false);
        }

        public ICrudRepo<TblProduct> GetRepo()
        {
            return _repo;
        }

        public async Task EditProduct(ProductFormModel data)
        {
            var existing = await _context.TblProduct.FirstOrDefaultAsync(p => p.ProductId == data.Product.ProductId);
            if (existing != null)
            {
                existing.ProductName = data.Product.ProductName;
                existing.Price = data.Product.Price;
                existing.CategoryId = data.Product.CategoryId;
                existing.ProductDescription = data.Product.ProductDescription;
                existing.ProductStock = data.Product.ProductStock;
                existing.ProductImage = data.Product.ProductImage;
                existing.ImageType = data.Product.ImageType;
                existing.IsAvailable = data.Product.IsAvailable;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ProductFormModel> EditProductModal(string id)
        {
            var product = await _context.TblProduct
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id).ConfigureAwait(false);

            var productCategories = await _context.TblProductCategory
                .ToListAsync().ConfigureAwait(false);

            return new ProductFormModel
            {
                Product = product,
                ProductCategories = productCategories
            };
        }
    }
}
