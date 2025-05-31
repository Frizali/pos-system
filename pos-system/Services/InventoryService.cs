using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class InventoryService(IInventoryRepo inventoryRepo) : IInventoryService
    {
        IInventoryRepo _inventoryRepo = inventoryRepo;
        readonly int codeLength = 4;

        public async Task<InventoryViewModel> GetListPart(string search, string searchPartType)
        {
            InventoryViewModel list = await _inventoryRepo.GetListPart(search, searchPartType).ConfigureAwait(false);
            return list;
        }

        public async Task Save(InventoryFormModel data)
        {
            data.Part.PartCd = Unique.GenerateCode(data.Part.PartName, codeLength);

            await _inventoryRepo.GetRepo().Add(data.Part).ConfigureAwait(false);
        }

        public async Task<InventoryFormModel> GetPartTypeAndUnit()
        {
            var result = await _inventoryRepo.GetPartTypeAndUnit().ConfigureAwait(false);
            return result;
        }

        public async Task DeletePart(string id)
        {
            await _inventoryRepo.DeletePart(id).ConfigureAwait(false);
        }
    }
}
