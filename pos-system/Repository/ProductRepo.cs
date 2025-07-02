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
            var productVariants = await _context.TblProductVariant.ProjectTo<ProductVariantDTO>(_mapper.ConfigurationProvider).ToListAsync().ConfigureAwait(false);

            var result = (from p in products
                          join pc in productCategories on p.CategoryId equals pc.CategoryId into pcGroup
                          from pc in pcGroup.DefaultIfEmpty()
                          select new ProductDTO
                          {
                              ProductId = p.ProductId,
                              ProductCode = p.ProductCode,
                              ProductName = p.ProductName,
                              ProductDescription = p.ProductDescription,
                              ProductStock = p.ProductStock,
                              IsLimitedStock = p.IsLimitedStock,
                              IsAvailable = p.IsAvailable,
                              Price = p.Price,
                              IsRecommended = p.IsRecommended,
                              CategoryId = p.CategoryId,
                              Category = pc,
                              ImageType = p.ImageType,
                              ProductImage = p.ProductImage,
                              ProductVariants = new List<ProductVariantDTO>()
                          }).ToList();

            foreach (var product in result)
            {
                product.ProductVariants = productVariants.Where(pv => pv.ProductId == product.ProductId).ToList();
            }

            return result;
        }

        public async Task<TblProduct> ProductDetailByID(string id)
        {
            return await _context.TblProduct
                .Where(p => p.ProductId == id)
                .Include(p => p.Category)
                .Include(p => p.TblVariantGroups)
                    .ThenInclude(v => v.TblVariantOptions)
                .Include(p => p.TblProductVariants)
                .FirstAsync().ConfigureAwait(false);
        }

        public ICrudRepo<TblProduct> GetRepo()
        {
            return _repo;
        }

        public async Task EditProduct(ProductFormModel data, string username)
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

            if(data.ProductVariants != null)
            {
                var variantFromDb = await _context.TblProductVariant.Where(x => x.ProductId == data.Product.ProductId).ToListAsync().ConfigureAwait(false);

                var variantIdFromWeb = data.ProductVariants.Select(x => x.VariantId);
                var variantIdFromDb = variantFromDb.Select(x => x.VariantId);

                var variantUpdated = variantFromDb.Where(x => variantIdFromWeb.Contains(x.VariantId));
                var variantAdded = data.ProductVariants.Where(x => !variantIdFromDb.Contains(x.VariantId));
                var variantDeleted = variantFromDb.Where(x => !variantIdFromWeb.Contains(x.VariantId));

                foreach (var variantData in variantUpdated)
                {
                    var updatedData = data.ProductVariants.FirstOrDefault(x => x.VariantId == variantData.VariantId);
                    if (updatedData == null) continue;
                    variantData.Sku = updatedData.Sku;
                    variantData.VariantPrice = updatedData.VariantPrice;
                    variantData.VariantStock = updatedData.VariantStock;
                    variantData.IsLimitedStock = updatedData.IsLimitedStock;
                    variantData.IsAvailable = updatedData.IsAvailable;
                }

                foreach (var variantData in variantAdded)
                {
                    variantData.ProductId = data.Product.ProductId;
                    await _context.TblProductVariant.AddAsync(variantData).ConfigureAwait(false);
                }

                foreach (var variantData in variantDeleted)
                {
                    _context.TblProductVariant.Remove(variantData);
                }
            }

            if (data.ProductAddons != null)
            {
                var AddonFromDb = await _context.TblProductAddon.Where(x => x.ProductId == data.Product.ProductId).ToListAsync().ConfigureAwait(false);

                var AddonIdFromWeb = data.ProductAddons.Select(x => x.AddonId);
                var AddonIdFromDb = AddonFromDb.Select(x => x.AddonId);

                var AddonUpdated = AddonFromDb.Where(x => AddonIdFromWeb.Contains(x.AddonId));
                var AddonAdded = data.ProductAddons.Where(x => !AddonIdFromDb.Contains(x.AddonId));
                var AddonDeleted = AddonFromDb.Where(x => !AddonIdFromWeb.Contains(x.AddonId));

                foreach (var AddonData in AddonUpdated)
                {
                    var updatedData = data.ProductAddons.FirstOrDefault(x => x.AddonId == AddonData.AddonId);
                    if (updatedData == null) continue;
                    AddonData.AddonName = updatedData.AddonName;
                    AddonData.AddonPrice = updatedData.AddonPrice;
                    AddonData.AddonStock = updatedData.AddonStock;
                    AddonData.IsLimitedStock = updatedData.IsLimitedStock;
                    AddonData.IsAvailable = updatedData.IsAvailable;
                }

                foreach (var AddonData in AddonAdded)
                {
                    AddonData.ProductId = data.Product.ProductId;
                    await _context.TblProductAddon.AddAsync(AddonData).ConfigureAwait(false);
                }

                foreach (var AddonData in AddonDeleted)
                {
                    _context.TblProductAddon.Remove(AddonData);
                }
            }

            await _context.SaveChangesAsync(username).ConfigureAwait(false);
        }

        public async Task<ProductFormModel> EditData(string id)
        {
            var product = await _context.TblProduct
                .Include(p => p.Category)
                .FirstAsync(p => p.ProductId == id).ConfigureAwait(false);

            var productCategories = await _context.TblProductCategory
                .ToListAsync().ConfigureAwait(false);

            var productVariants = await _context.TblProductVariant.Where(x => x.ProductId == product.ProductId).ToListAsync().ConfigureAwait(false);

            var productAddon = await _context.TblProductAddon.Where(x => x.ProductId == product.ProductId).ToListAsync().ConfigureAwait(false);

            return new ProductFormModel
            {
                Product = product,
                ProductCategories = productCategories,
                ProductVariants = productVariants,
                ProductAddons = productAddon
            };
        }
    }
}
