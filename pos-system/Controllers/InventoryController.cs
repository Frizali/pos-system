using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

namespace pos_system.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Authorize]

    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            var inventoryList = new List<InventoryViewModel>
            {
                new InventoryViewModel { Id = 1, NamaBarang = "Teh Botol", Kategori = "jadi", Unit = "10 pcs", Harga = 40000, Note = "taruh di suhu ruangan di bawah 30 derajat'C " },
                new InventoryViewModel { Id = 2, NamaBarang = "Indomie Goreng", Kategori = "jadi", Unit = "45 pcs", Harga = 135000 },
                new InventoryViewModel { Id = 3, NamaBarang = "Ayam", Kategori = "Mentah", Unit = "5 kg", Harga = 175000 }
            };

            return View(inventoryList);
        }
        public IActionResult AddInventory()
        {
            return View();
        }

        public IActionResult InventoryLog()
        {
            // data dummy
            var inventoryLogs = new List<InventoryLogViewModel>
            {
                new InventoryLogViewModel {
                    NamaBarang  = "Ayam",
                    Kategori    = "Mentah",
                    StockIn     = 10,
                    StockOut    = 0,
                    Note        = "Restock gudang pusat",
                    InputedBy   = "Admin",
                    CreatedAt   = DateTime.Now.AddDays(-1)
                },
                new InventoryLogViewModel {
                    NamaBarang  = "Beras",
                    Kategori    = "Mentah",
                    StockIn     = 0,
                    StockOut    = 5,
                    Note        = "Penjualan",
                    InputedBy   = "Kasir",
                    CreatedAt   = DateTime.Now.AddDays(-2)
                },
                new InventoryLogViewModel {
                    NamaBarang  = "Teh Botol",
                    Kategori    = "jadi",
                    StockIn     = 20,
                    StockOut    = 0,
                    Note        = "Pembelian distributor",
                    InputedBy   = "Admin",
                    CreatedAt   = DateTime.Now.AddDays(-3)
                }
            };

            ViewBag.Products = new List<string> { "Ayam", "Beras", "Teh Botol" };
            ViewBag.Categories = new List<string> { "Mentah", "jadi" };

            return View(inventoryLogs);
        }

        public IActionResult ExportPdf()
            => Content("PDF export belum di-implement");

        public IActionResult ExportExcel()
            => Content("Excel export belum di-implement");

    }
}
