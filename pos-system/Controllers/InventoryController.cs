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
            try
            {
                var userName = GetCurrentUserName();
                data.InputedBy = userName;
                await _inventoryService.AddPartMovement(data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var modelError = ex.Message.Split(",");
                ModelState.AddModelError(modelError[0], modelError[1].Trim());

                await GetEditStockModal(data.PartId);
                return PartialView("EditStockModal", data);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetListPartMovement(string partId, string partTypeId, DateTime date, string month, string year)
        {
            var result = await _inventoryService.GetListPartMovement(partId, partTypeId, date, month, year).ConfigureAwait(false);

            return View("InventoryLog", result);
        }

        public IActionResult ExportPdf()
            => Content("PDF export belum di-implement");

        public IActionResult ExportExcel()
            => Content("Excel export belum di-implement");

        private string GetCurrentUserName()
        {
            return User.Identity?.Name ?? "System";
        }
    }
}
