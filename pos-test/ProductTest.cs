using AutoMapper;
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

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=POS;");
            _context = new AppDbContext(optionsBuilder.Options);
            _productId = Unique.ID();
        }

        [OneTimeTearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var crudCategoryRepo = new CrudRepo<TblProductCategory>(_context);
            var crudProductRepo = new CrudRepo<TblProduct>(_context);
            var categoryRepo = new ProductCategoryRepo(_context, null, crudCategoryRepo);
            var productRepo = new ProductRepo(_context, null, crudProductRepo);
            var variantGroupRepo = new VariantGroupRepo(null);
            var orderNumberRepo = new OrderNumberTrackerRepo(_context);

            var service = new ProductService(
                categoryRepo,
                productRepo,
                variantGroupRepo,
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
                    ProductName = "TP02",
                    ProductDescription = "TP02",
                    ProductCode = "TP02",
                    CategoryId = "066B57ED-1AB7-4F1B-A596-B9A14114DBA9",
                    Price = 10000,
                    ProductStock = 10,
                }
            };

            await _service.Save(data, null);
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

        [Test]
        public async Task Add_ProductWithVariant_ShouldBeAdded()
        {
            ProductFormModel data = new()
            {
                Product = new TblProduct
                {
                    ProductId = _productId,
                    ProductName = "TP03",
                    ProductDescription = "TP03",
                    ProductCode = "TP03",
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
                        ProductId = _productId,
                        TblVariantOptions = new List<TblVariantOption>
                        {
                            new TblVariantOption
                            {
                                Value = "Kecil"
                            },
                            new TblVariantOption
                            {
                                Value = "Besar"
                            }
                        }
                    }
                }
            };

            await _service.Save(data, null);
        }

        [Test]
        public async Task Get_ProductDetailById_ShouldReturnSameProduct()
        {
            var result = await _service.ProductDetailByID(_productId);

            Assert.That(result, Is.Not.Null);
        }
    }
}