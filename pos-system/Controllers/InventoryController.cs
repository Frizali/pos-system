using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pos_system.Models;
using pos_system.Repository;
using pos_system.Services;

namespace pos_system.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Authorize]

    public class InventoryController(IInventoryService inventoryService) : Controller
    {
        readonly IInventoryService _inventoryService = inventoryService;

        public async Task<IActionResult> Index(string search, string searchPartType)
        {
            var result = await _inventoryService.GetListPart(search,searchPartType).ConfigureAwait(false);

            return View("Index", result);
        }

        public async Task<IActionResult> AddInventory(InventoryFormModel data)
        {
            if (data.Part != null)
            {
                try
                {
                    await _inventoryService.Save(data).ConfigureAwait(false);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var modelError = ex.Message.Split(",");
                    ModelState.AddModelError("Inventory." + modelError[0], modelError[1]);

                    var listData = await _inventoryService.GetPartTypeAndUnit();
                    data.PartTypes = listData.PartTypes;
                    data.Units = listData.Units;

                    return View("AddInventory", data);
                }
            }
            else
            {
                var listData = await _inventoryService.GetPartTypeAndUnit();
                data.PartTypes = listData.PartTypes;
                data.Units = listData.Units;

                return View("AddInventory", data);
            }
        }

        public async Task<IActionResult> DeletePartConfirmed(string id)
        {
            try
            {
                await _inventoryService.DeletePart(id).ConfigureAwait(false);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> LoadEditModal(string id)
        {
            var result = await _inventoryService.LoadEditModal(id).ConfigureAwait(false);
            return PartialView("EditModal", result);
        }

        public async Task<IActionResult> Update(InventoryFormModel data)
        {
            await _inventoryService.Update(data).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetEditStockModal(string partId)
        {
            var result = await _inventoryService.GetEditStockModal(partId).ConfigureAwait(false);
            return PartialView("EditStockModal", result);
        }

        public async Task<IActionResult> AddPartMovement(EditStockFormModal data)
        {
            var userName = GetCurrentUserName();
            data.InputedBy = userName;
            await _inventoryService.AddPartMovement(data).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
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
