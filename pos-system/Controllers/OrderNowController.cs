using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

namespace pos_system.Controllers
{
    public class OrderNowController : Controller
    {
        public IActionResult Index()
        {
            var listPO = new List<POModel>
            {
                GetSamplePO(1, "Customer A", "Waiting"),
                GetSamplePO(2, "Customer B", "Waiting")
            };
            return View(listPO);
        }

        public IActionResult Process()
        {
            var listPO = new List<POModel>
            {
                GetSamplePO(3, "Customer C", "Process"),
                GetSamplePO(6, "Customer F", "Process")
            };
            return View(listPO);
        }

        public IActionResult Status()
        {
            var listPO = new List<POModel>
            {
                GetSamplePO(4, "Customer D", "Delivered"),
                GetSamplePO(7, "Customer G", "Delivered")
            };
            return View(listPO);
        }

        [HttpPost]
        public IActionResult Approve(int orderId, string note)
        {
            TempData["Message"] = $"PO ID {orderId} telah di-process dengan catatan: {note}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int orderId, string note)
        {
            TempData["Message"] = $"PO ID {orderId} telah di-reject dengan catatan: {note}";
            return RedirectToAction("Index");
        }

        private POModel GetSamplePO(int id, string name, string status)
        {
            return new POModel
            {
                Id = id,
                Name = name,
                InvoiceNumber = GenerateInvoiceNumber(id),
                TanggalPO = DateTime.Now.AddDays(1),
                Note = "Catatan pesanan",
                Total = 56000,
                Status = status,
                Detail = new List<PODetail>
                {
                    new PODetail { Produk = "Nasi Bakar", Variant = "Besar", Note = "Pedas", Qty = 2 },
                    new PODetail { Produk = "Wedang Jahe", Variant = "Sedang", Note = "Manis", Qty = 1 }
                }
            };
        }

        private string GenerateInvoiceNumber(int id)
        {
            // Format: INV20250623-001
            string tanggal = DateTime.Now.ToString("yyyyMMdd");
            return $"INV{tanggal}-{id:000}";
        }
    }
}
