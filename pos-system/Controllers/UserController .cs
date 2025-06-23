using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

namespace pos_system.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PoOrderTable()
        {
            var dummyOrders = new List<POModel>
        {
            new POModel
            {
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
                Name = "Rina Marlina",
                TanggalPO = DateTime.Today.AddDays(-1),
                Note = "Dibungkus rapat",
                Total = 10000,
                Status = "Selesai",
                Detail = new List<PODetail>
                {
                    new PODetail { Produk = "Wedang Jahe", Variant = "Kecil", Note = "Tanpa gula", Qty = 1 }
                }
            }
        };

            return View("PoOrderTable", dummyOrders);
        }
        public IActionResult ManageAcc()
        {
            return View();
        }
    }
}
