using Microsoft.AspNetCore.Mvc;

namespace pos_system.Controllers
{
    public class OrderPoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var dummyOrders = new List<POModel>
            {
                new POModel
                {
                    Id = 1,
                    Name = "Joko Santoso",
                    TanggalPO = DateTime.Today,
                    Note = "Diantar sebelum jam 7 malam",
                    Total = 35000,
                    Status = "Diproses",
                    Detail = new List<PODetail>
                    {
                        new PODetail { Produk = "Sate Usus", Variant = "Besar", Note = "Pedas", Qty = 3 },
                        new PODetail { Produk = "Nasi Kucing", Variant = "Sedang", Note = "Tambah sambel", Qty = 2 }
                    }
                },
                new POModel
                {
                    Id = 2,
                    Name = "Rina Marlina",
                    TanggalPO = DateTime.Today.AddDays(-1),
                    Note = "Dibungkus rapat",
                    Total = 10000,
                    Status = "Menunggu",
                    Detail = new List<PODetail>
                    {
                        new PODetail { Produk = "Wedang Jahe", Variant = "Kecil", Note = "Tanpa gula", Qty = 1 }
                    }
                }
            };

            return View(dummyOrders); 
        }

        [HttpPost]
        public IActionResult Approve(int OrderId, string Note)
        {
            TempData["Message"] = $"Order #{OrderId} disetujui: {Note}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int OrderId, string Note)
        {
            TempData["Message"] = $"Order #{OrderId} ditolak: {Note}";
            return RedirectToAction("Index");
        }
    }
}
