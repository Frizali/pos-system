using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using pos_system.Data;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;
using pos_system.Services;

namespace pos_test
{
    public class ProductTest
    {
        private AppDbContext _context;
        private ProductService _service;
        private string _productId;
        private string _filePath;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=POS;");
            _context = new AppDbContext(optionsBuilder.Options);
            _productId = Unique.ID();
            _filePath = "C:\\Users\\Rizal\\Pictures\\pngtree-coffee-cup-coffee-white-background-transparent-png-image_6673498-2216671691.png";
        }

        [OneTimeTearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var mockCateRepo = new Mock<IProductCategoryRepo>();
            mockCateRepo.Setup(repo => repo.GetRepo()).Returns(new CrudRepo<TblProductCategory>(_context));
            var mockProductRepo = new Mock<IProductRepo>();
            mockProductRepo.Setup(repo => repo.GetRepo()).Returns(new CrudRepo<TblProduct>(_context));
            var mockVariantGroupRepo = new Mock<IVariantGroupRepo>();
            mockVariantGroupRepo.Setup(repo => repo.GetRepo()).Returns(new CrudRepo<TblVariantGroup>(_context));
            var orderNumberRepo = new OrderNumberTrackerRepo(_context);

            var service = new ProductService(
                mockCateRepo.Object,
                mockProductRepo.Object,
                mockVariantGroupRepo.Object,
                orderNumberRepo
            );

            _service = service;
        }

        [Test]
        public async Task Add_ProductWithoutVariant_ShouldBeAdded()
        {
            ProductFormModel data = new()
            {
                Product = new TblProduct
                {
                    ProductId = _productId,
                    ProductName = "TP01",
                    ProductDescription = "TP01",
                    ProductCode = "TP01",
                    CategoryId = "753A1C7F-9F91-4E1E-AEAA-B152259DDE91",
                    Price = 10000,
                    ProductStock = 10,
                }
            };

            await _service.Save(data, CreateImageFormFile(_filePath, "coffie"));
            var result = await _service.ProductDetailByID(_productId);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ProductId, Is.EqualTo(data.Product.ProductId));
                Assert.That(result.ProductName, Is.EqualTo(data.Product.ProductName));
                Assert.That(result.ProductCode, Is.EqualTo(data.Product.ProductCode));
                Assert.That(result.CategoryId, Is.EqualTo(data.Product.CategoryId));
                Assert.That(result.Price, Is.EqualTo(data.Product.Price));
                Assert.That(result.ProductStock, Is.EqualTo(data.Product.ProductStock));
            });
        }

        public IFormFile CreateImageFormFile(string filePath, string contentType = "image/png")
        {
            var fileStream = File.OpenRead(filePath);
            return new FormFile(fileStream, 0, fileStream.Length, "productImage", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        [Test]
        public async Task Add_ProductWithVariant_ShouldBeAdded()
        {
            var newProductId = Unique.ID();

            ProductFormModel data = new()
            {
                Product = new TblProduct
                {
                    ProductId = newProductId,
                    ProductName = "TP02",
                    ProductDescription = "TP02",
                    ProductCode = "TP02",
                    CategoryId = "A726CF47-0C4F-4506-8023-CC244E7A1290",
                    Price = 15000,
                    ProductStock = 20,
                },
                ProductVariants = new List<TblProductVariant>
                {
                    new TblProductVariant
                    {
                        Sku = "Kecil",
                        VariantPrice = 15000,
                        VariantStock = 100,
                        IsLimitedStock = true,
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new TblProductVariant
                    {
                        Sku = "Besar",
                        VariantPrice = 15000,
                        VariantStock = 100,
                        IsLimitedStock = true,
                        IsAvailable = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                },
                VariantGroups = new List<TblVariantGroup>
                {
                    new TblVariantGroup
                    {
                        VariantName = "Size",
                        ProductId = newProductId
                    }
                }
            };


            await _service.Save(data, CreateImageFormFile(_filePath, "coffie"));
            var result = await _service.ProductDetailByID(newProductId);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ProductId, Is.EqualTo(data.Product.ProductId));
                Assert.That(result.ProductName, Is.EqualTo(data.Product.ProductName));
                Assert.That(result.ProductCode, Is.EqualTo(data.Product.ProductCode));
                Assert.That(result.CategoryId, Is.EqualTo(data.Product.CategoryId));
                Assert.That(result.Price, Is.EqualTo(data.Product.Price));
                Assert.That(result.ProductStock, Is.EqualTo(data.Product.ProductStock));
                Assert.That(result.TblProductVariants.Count, Is.EqualTo(data.ProductVariants.Count));
                foreach (var (variant, index) in data.ProductVariants.Select((v,i) => (v,i)))
                {
                    Assert.That(result.TblProductVariants.ToList()[index].Sku, Is.EqualTo(variant.Sku));
                    Assert.That(result.TblProductVariants.ToList()[index].VariantPrice, Is.EqualTo(variant.VariantPrice));
                    Assert.That(result.TblProductVariants.ToList()[index].VariantStock, Is.EqualTo(variant.VariantStock));
                    Assert.That(result.TblProductVariants.ToList()[index].IsLimitedStock, Is.EqualTo(variant.IsLimitedStock));
                    Assert.That(result.TblProductVariants.ToList()[index].IsAvailable, Is.EqualTo(variant.IsAvailable));
                }
            });
        }

        [Test]
        public async Task Get_ProductDetailById_ShouldReturnSameProduct()
        {
            var result = await _service.ProductDetailByID(_productId);

            Assert.That(result, Is.Not.Null);
        }
    }
}