using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class ProductService(IProductCategoryRepo categoryRepo, IProductRepo productRepo, IVariantGroupRepo variantGroupRepo, IOrderRepo orderRepo, IOrderNumberTrackerRepo orderNumberTrackerRepo) : IProductService
    {
        readonly IProductCategoryRepo _categoryRepo = categoryRepo;
        readonly IProductRepo _productRepo = productRepo;
        readonly IVariantGroupRepo _variantGroupRepo = variantGroupRepo;
        readonly IOrderRepo _orderRepo = orderRepo;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;
        readonly int codeLength = 4;
        private string _username = "System";

        public void SetUsername(string username)
        {
            _username = username;
            _categoryRepo.GetRepo().SetUsername(username);
            _productRepo.GetRepo().SetUsername(username);
            _variantGroupRepo.GetRepo().SetUsername(username);
        }

        public async Task<ProductFormModel> ProductFormModel()
        {
            List<TblProductCategory> productCategories = await _categoryRepo.GetRepo().GetAll().ConfigureAwait(false);
            return new ProductFormModel
            {
                ProductCategories = productCategories
            };
        }

        public async Task<ProductListViewModel> ProductListViewModel(string? category, string? product)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            var fromDate = new DateTime(year, month, DateTime.Now.Day).AddDays(-6).ToString("yyyy-MM-dd");
            var toDate = new DateTime(year, month, DateTime.Now.Day).ToString("yyyy-MM-dd");

            var orders = await _orderRepo.GetOrdersByDate(fromDate, toDate);
            var productDTOs = await _productRepo.ProductDetailsDTO();

            var favoriteProducts = GetFavoriteProduct(orders, productDTOs);
            var products = await _productRepo.ProductDetailsDTO().ConfigureAwait(false);
            var productCategoriesDTO = await _categoryRepo.ProductCategoriesDTO().ConfigureAwait(false);
            var orderNumber = await _orderNumberTrackerRepo.GetOrderNumber().ConfigureAwait(false);

            products = (from p in products join fp in favoriteProducts on p.ProductId equals fp.ProductId into fpGroup
                       from fp in fpGroup.DefaultIfEmpty()
                       select new ProductDTO() {
                           ProductId = p.ProductId,
                           ProductName = p.ProductName,
                           CategoryId = p.CategoryId,
                           Category = p.Category,
                           Price = p.Price,
                           ProductImage = p.ProductImage,
                           ImageType = p.ImageType,
                           IsAvailable = p.IsAvailable,
                           ProductDescription = p.ProductDescription,
                           ProductStock = p.ProductStock,
                           ProductCode = p.ProductCode,
                           ProductVariants = p.ProductVariants,
                           IsLimitedStock = p.IsLimitedStock,
                           IsRecommended = fp != null && fp.TotalOrder > 0,
                           CreatedAt = p.CreatedAt,
                           UpdatedAt = p.UpdatedAt
                       }).ToList();

            ProductCategoryDTO allProductCategory = new ()
            {
                CategoryId = "All",
                CategoryName = "All",
                CategoryDescription = "All",
                TotalProducts = productCategoriesDTO.Sum(pc => pc.TotalProducts)
            };

            productCategoriesDTO.Add(allProductCategory);

            if (!String.IsNullOrEmpty(category) && category != "All")
                products = products.Where(p => p.CategoryId == category).ToList();

            if (!String.IsNullOrEmpty(product))
                products = products.Where(p => p.ProductName.Contains(product, StringComparison.OrdinalIgnoreCase)).ToList();

            return new ProductListViewModel
            {
                Products = products,
                ProductCategories = productCategoriesDTO,
                OrderNumber = orderNumber,
                ProductFilter = product
            };
        }

        public async Task Save(ProductFormModel data, IFormFile? productImage)
        {
            data.Product.ProductCode = Unique.GenerateCode(data.Product.ProductName,codeLength);
            SetVariant(data);

            await SetImage(data, productImage).ConfigureAwait(false);
            await _productRepo.GetRepo().Add(data.Product).ConfigureAwait(false);
            await SetVariantOptions(data).ConfigureAwait(false);
        }

        private static async Task SetImage(ProductFormModel data, IFormFile? productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                int maxSize = 400 * 1024;

                if (productImage.Length > maxSize)
                    throw new Exception("ProductImage, Image size exceeds the maximum limit of 400KB.");

                using var memoryStream = new MemoryStream();
                await productImage.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                data.Product.ProductImage = Convert.ToBase64String(imageData);
                data.Product.ImageType = Path.GetExtension(productImage.FileName)?.ToLower();
            }
        }

        private static void SetVariant(ProductFormModel data)
        {
            if (data.VariantGroups is not null && data.VariantGroups.Count > 0)
                data.VariantGroups = data.VariantGroups.Select(x => { x.ProductId = data.Product.ProductId; return x; }).ToList();

            if (data.ProductVariants is not null && data.ProductVariants.Count > 0)
                data.Product.TblProductVariants = data.ProductVariants.Select(x => { x.ProductId = data.Product.ProductId; x.Sku = x.Sku.Trim(); return x; }).ToList();

            if (data.ProductAddons is not null && data.ProductAddons.Count > 0)
                data.Product.TblProductAddons = data.ProductAddons.Select(x => { x.ProductId = data.Product.ProductId; return x; }).ToList();
        }

        private async Task SetVariantOptions(ProductFormModel data)
        {
            if(data.ProductVariants is not null && data.ProductVariants.Count > 0 && data.VariantGroups is not null && data.VariantGroups.Count > 0)
            {
                var variantOptionSplit = data.ProductVariants.Select(x => x.Sku.Split("-")).ToList();
                var totalVariant = variantOptionSplit.First().Count();

                for (var i = 0; i < totalVariant; i++)
                {
                    var variantOption = variantOptionSplit.Select(x => x[i].Trim()).Distinct().ToList();
                    var variantGroup = data.VariantGroups[i];
                    var variantGroupId = variantGroup.GroupId;

                    foreach (var option in variantOption)
                    {
                        TblVariantOption tblVariantOption = new()
                        {
                            OptionId = Unique.ID(),
                            GroupId = variantGroupId,
                            Value = option
                        };

                        variantGroup.TblVariantOptions.Add(tblVariantOption);
                    }
                }

                await _variantGroupRepo.GetRepo().AddRange(data.VariantGroups).ConfigureAwait(false);
            }
        }

        public async Task<TblProduct> ProductDetailByID(string id)
        {
            return await _productRepo.ProductDetailByID(id).ConfigureAwait(false);
        }

        public async Task<List<PreOrderItemDTO>> GetPreOrderItem(TblOrder order)
        {
            var productDTOs = await _productRepo.ProductDetailsDTO().ConfigureAwait(false);
            var orderProductID = order.TblOrderItems.Where(oi => oi.VariantId is null).Select(oi => new { ProductID = oi.ProductId, oi.Quantity }).ToList();
            var orderVariantID = order.TblOrderItems.Where(oi => oi.VariantId is not null).Select(oi => new { ProductID = oi.ProductId, VariantID = oi.VariantId, oi.Quantity }).ToList();

            var resProduct = from op in orderProductID
                             join p in productDTOs on op.ProductID equals p.ProductId
                             select new
                             {
                                 op,
                                 p
                             } into joined
                             group joined by joined.op.ProductID into g
                             select new PreOrderItemDTO()
                             {
                                 ProductName = g.First().p.ProductName,
                                 Quantity = g.Sum(x => x.op.Quantity),
                             };

            var resVariant = from ov in orderVariantID
                             join v in productDTOs.SelectMany(p => p.ProductVariants).ToList() on ov.VariantID equals v.VariantId
                             join p in productDTOs on v.ProductId equals p.ProductId
                             select new { p, v, ov } into vpJoined
                             group vpJoined by vpJoined.v.VariantId into g
                             select new PreOrderItemDTO()
                             {
                                 ProductName = g.First().p.ProductName + $" ({g.First().v.Sku})",
                                 Quantity = g.Sum(x => x.ov.Quantity),
                             }; 

            return resProduct.Concat(resVariant).ToList();
        }


        public async Task EditProduct(ProductFormModel data, IFormFile? productImage)
        {
            await SetImage(data, productImage).ConfigureAwait(false);
            await _productRepo.EditProduct(data, _username).ConfigureAwait(false);
        }

        public async Task<ProductFormModel> EditData(string id)
        {
            return await _productRepo.EditData(id).ConfigureAwait(false);
        }

        private List<FavoriteProduct> GetFavoriteProduct(List<TblOrder> orders, List<ProductDTO> productDTOs)
        {
            var orderProductID = orders.SelectMany(o => o.TblOrderItems).Select(oi => new { ProductID = oi.ProductId, oi.Quantity }).ToList();

            var resProduct = from op in orderProductID
                             join p in productDTOs on op.ProductID equals p.ProductId
                             select new
                             {
                                 op,
                                 p
                             } into joined
                             group joined by joined.op.ProductID into g
                             select new FavoriteProduct()
                             {
                                 ProductId = g.First().p.ProductId,
                                 ProductName = g.First().p.ProductName,
                                 CategoryName = g.First().p.Category.CategoryName,
                                 TotalOrder = g.Sum(x => x.op.Quantity),
                             };

            return resProduct.OrderByDescending(fp => fp.TotalOrder).Take(5).ToList();
        }
    }
}
