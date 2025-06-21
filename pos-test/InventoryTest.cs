using AutoMapper;
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
    public class InventoryTest
    {
        private AppDbContext _context;
        private InventoryService _service;
        private string _partId;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-C24PFO2E\\SQLEXPRESS;Database=POS;Trusted_Connection=true;TrustServerCertificate=True;");
            _context = new AppDbContext(optionsBuilder.Options);
            _partId = Unique.ID();
        }

        [OneTimeTearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var crudInventoryRepo = new CrudRepo<TblPart>(_context);
            var inventoryRepo = new InventoryRepo(_context, null, crudInventoryRepo);

            var service = new InventoryService(
                inventoryRepo
            );

            _service = service;
        }

        [Test]
        public async Task AddInventoryPart_ShouldAddPartCorrectly()
        {
            InventoryFormModel data = new()
            {
                Part = new TblPart
                {
                    PartId = _partId,
                    PartTypeId = "5dd4b2f1-7a82-4b03-897f-aab1ef608b1d",
                    UnitId = "804c73ad-2a57-45ee-b2b7-299a3d8c538d",
                    PartName = "Tepung Terigu",
                    PartCd = Unique.GenerateCode("Tepung Terigu", 6),
                    PartQty = 5,
                    Price = 50000,
                    Note = "Periksa kondisi kemasan dan simpan di tempat yang kering dan tertutup rapat."
                }
            };

            await _service.Save(data);
            var result = await _service.LoadEditModal(_partId);

            Assert.That(result.Part, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Part.PartId, Is.EqualTo(data.Part.PartId));
                Assert.That(result.Part.PartTypeId, Is.EqualTo(data.Part.PartTypeId));
                Assert.That(result.Part.UnitId, Is.EqualTo(data.Part.UnitId));
                Assert.That(result.Part.PartName, Is.EqualTo(data.Part.PartName));
                Assert.That(result.Part.PartCd, Is.EqualTo(data.Part.PartCd));
                Assert.That(result.Part.PartQty, Is.EqualTo(data.Part.PartQty));
                Assert.That(result.Part.Price, Is.EqualTo(data.Part.Price));
                Assert.That(result.Part.Note, Is.EqualTo(data.Part.Note));
            });
        }

        [Test]
        public async Task EditInventoryPart_ShouldUpdatePartCorrectly()
        {
            var partId = "c5b1eb0c-bd5e-4672-98be-8ef34db7f40a";
            var data = await _service.LoadEditModal(partId);
            InventoryFormModel updatedData = new()
            {
                Part = new TblPart
                {
                    PartId = partId,
                    PartTypeId = "5dd4b2f1-7a82-4b03-897f-aab1ef608b1d",
                    UnitId = "804c73ad-2a57-45ee-b2b7-299a3d8c538d",
                    PartName = "Tepung Terigu Premium",
                    PartCd = data.Part.PartCd,
                    PartQty = 10,
                    Price = 120000,
                    Note = "Ubah catatan: simpan di tempat dingin."
                }
            };

            await _service.Update(updatedData);

            var result = await _service.LoadEditModal(partId);
            Assert.Multiple(() =>
            {
                Assert.That(result.Part, Is.Not.Null);
                Assert.That(result.Part.PartId, Is.EqualTo(updatedData.Part.PartId));
                Assert.That(result.Part.PartTypeId, Is.EqualTo(updatedData.Part.PartTypeId));
                Assert.That(result.Part.UnitId, Is.EqualTo(updatedData.Part.UnitId));
                Assert.That(result.Part.PartName, Is.EqualTo(updatedData.Part.PartName));
                Assert.That(result.Part.PartQty, Is.EqualTo(updatedData.Part.PartQty));
                Assert.That(result.Part.Price, Is.EqualTo(updatedData.Part.Price));
                Assert.That(result.Part.Note, Is.EqualTo(updatedData.Part.Note));
            });
        }

        [Test]
        public async Task DeleteInventoryPart_ShouldRemovePart()
        {
            var partId = "9ff7273a-2263-401c-a78b-3e016c9f640c";

            await _service.DeletePart(partId);

            var result = await _service.LoadEditModal(partId);
            Assert.That(result.Part, Is.Null, "Successfully delete part");
        }
    }
}
