using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;
using pos_system.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_test
{
    public class OrderTest
    {
        private AppDbContext _context;
        private OrderService _service;
        private string _orderId;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-C24PFO2E\\SQLEXPRESS;Database=POS;Trusted_Connection=true;TrustServerCertificate=True;");
            _context = new AppDbContext(optionsBuilder.Options);
            _orderId = Unique.ID();
        }

        [OneTimeTearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var crudOrderRepo = new CrudRepo<TblOrder>(_context);
            var crudProdVariantRepo = new CrudRepo<TblProductVariant>(_context);
            var crudProdRepo = new CrudRepo<TblProduct>(_context);

            var orderRepo = new OrderRepo(_context, crudOrderRepo);
            var prodVariantRepo = new ProductVariantRepo(_context, crudProdVariantRepo);
            var prodRepo = new ProductRepo(_context, null, crudProdRepo);
            var orderNumTracRepo = new OrderNumberTrackerRepo(_context);

            var service = new OrderService(
                orderRepo,
                prodVariantRepo,
                prodRepo,
                orderNumTracRepo
            );

            _service = service;
        }

        [Test]
        public async Task Add_Order_ShouldBeAdded()
        {
            var orderNumTracRepo = new OrderNumberTrackerRepo(_context);

            TblOrderItem[] tblOrderItems =
            [
                new TblOrderItem { ProductId = "bfd581c3-f2ba-44f8-8e8c-2b314305a056", Quantity = 3, UnitPrice = 2000, Notes = "-" },
                new TblOrderItem { ProductId = "c41f2f36-9d8b-430d-8c3c-f20a4d448f0b", Quantity = 5, UnitPrice = 7000, Notes = "-" }
            ];

            TblOrder data = new()
            {
                OrderId = _orderId,
                Cashier = "admin",
                TblOrderItems = tblOrderItems
            };

            foreach (var orderItem in data.TblOrderItems)
            {
                orderItem.OrderId = data.OrderId;
                orderItem.SubTotal = orderItem.Quantity * orderItem.UnitPrice;
            }

            await _service.CreateOrder(data);

            var result = await _context.TblOrder.Where(x => x.OrderId == _orderId).FirstOrDefaultAsync();

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.OrderId, Is.EqualTo(data.OrderId));
                Assert.That(result.OrderNumber, Is.EqualTo(data.OrderNumber));
                Assert.That(result.OrderDate, Is.EqualTo(data.OrderDate));
                Assert.That(result.Status, Is.EqualTo(data.Status));
                Assert.That(result.TotalPrice, Is.EqualTo(data.TotalPrice));
                Assert.That(result.Cashier, Is.EqualTo(data.Cashier));
            });

            var orderItmResult = await _context.TblOrderItem.Where(x => x.OrderId == _orderId).ToListAsync();

            Assert.That(orderItmResult.Count, Is.EqualTo(tblOrderItems.Length));

            for (int i = 0; i < tblOrderItems.Length; i++)
            {
                var expected = tblOrderItems[i];
                var actual = orderItmResult.FirstOrDefault(x => x.ProductId == expected.ProductId);

                Assert.That(actual, Is.Not.Null, $"Item dengan ProductId {expected.ProductId} tidak ditemukan.");
                Assert.Multiple(() =>
                {
                    Assert.That(actual.Quantity, Is.EqualTo(expected.Quantity), $"Quantity mismatch untuk ProductId {expected.ProductId}");
                    Assert.That(actual.UnitPrice, Is.EqualTo(expected.UnitPrice), $"UnitPrice mismatch untuk ProductId {expected.ProductId}");
                    Assert.That(actual.SubTotal, Is.EqualTo(expected.Quantity * expected.UnitPrice), $"SubTotal mismatch untuk ProductId {expected.ProductId}");
                    Assert.That(actual.Notes, Is.EqualTo(expected.Notes), $"Notes mismatch untuk ProductId {expected.ProductId}");
                });
            }
        }
    }
}
